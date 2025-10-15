/*
 * RecommendationService.cs - Intelligent Recommendation Engine
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: Recommendation algorithm structure, preference scoring logic, and advanced data structure implementation guidance provided by AI assistant.
 * Student implementation: Core business logic, database integration, and service architecture.
 */
using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.DataStructures;

namespace MunicipalServicesApp.Services
{
    public class CategoryCount
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class SearchAnalyticsResult
    {
        public int TotalSearches { get; set; }
        public int SuccessfulSearches { get; set; }
        public double SuccessRate { get; set; }
        public LinkedList<CategoryCount> TopCategories { get; set; } = new LinkedList<CategoryCount>();
        public Queue<SearchHistory> RecentSearches { get; set; } = new Queue<SearchHistory>();
    }

    public class RecommendationService
    {
        private readonly ApplicationDbContext _context;

        public RecommendationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LinkedList<Event>> GetPersonalizedRecommendationsAsync(int maxRecommendations = 3)
        {
            var recommendations = new LinkedList<Event>();

            try
            {
                var userPreferences = await GetUserPreferencesAsync();
                
                if (!userPreferences.Any())
                {
                    var popularEvents = await GetPopularEventsAsync(maxRecommendations);
                    var fallbackResult = new LinkedList<Event>();
                    foreach (var eventItem in popularEvents)
                    {
                        fallbackResult.AddLast(eventItem);
                    }
                    return fallbackResult;
                }

                var topPreferences = userPreferences
                    .OrderByDescending(p => p.PreferenceScore)
                    .ThenByDescending(p => p.SearchCount)
                    .Take(3)
                    .ToList();

                foreach (var preference in topPreferences)
                {
                    var prefCat = preference.Category.ToLower();
                    var categoryEvents = await _context.Events
                        .Where(e => e.Category.ToLower() == prefCat)
                        .Where(e => e.Date >= DateTime.Now)
                        .OrderBy(e => e.Date)
                        .Take(maxRecommendations)
                        .ToListAsync();

                    foreach (var eventItem in categoryEvents)
                    {
                        recommendations.AddLast(eventItem);
                    }
                    
                    if (recommendations.Count >= maxRecommendations)
                        break;
                }

                if (recommendations.Count < maxRecommendations)
                {
                    var popularEvents = await GetPopularEventsAsync(maxRecommendations - recommendations.Count);
                    foreach (var eventItem in popularEvents)
                    {
                        recommendations.AddLast(eventItem);
                    }
                }

                var result = new LinkedList<Event>();
                var count = 0;
                foreach (var eventItem in recommendations)
                {
                    if (count >= maxRecommendations) break;
                    result.AddLast(eventItem);
                    count++;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating recommendations: {ex.Message}");
                var popularEvents = await GetPopularEventsAsync(maxRecommendations);
                var errorResult = new LinkedList<Event>();
                foreach (var eventItem in popularEvents)
                {
                    errorResult.AddLast(eventItem);
                }
                return errorResult;
            }
        }

        /// <summary>
        /// Updates user preferences based on search behavior
        /// </summary>
        public async Task UpdateUserPreferencesAsync(string? searchTerm, string? category, int resultCount)
        {
            try
            {
                // Record the search in history
                var searchHistory = new SearchHistory
                {
                    SearchTerm = searchTerm,
                    Category = category,
                    SearchDate = DateTime.Now,
                    ResultCount = resultCount,
                    SearchType = DetermineSearchType(searchTerm, category)
                };

                _context.SearchHistories.Add(searchHistory);

                // Update user preferences
                if (!string.IsNullOrEmpty(category))
                {
                    var cat = category.ToLower();
                    var preference = await _context.UserPreferences
                        .FirstOrDefaultAsync(p => p.Category.ToLower() == cat);

                    if (preference == null)
                    {
                        preference = new UserPreference
                        {
                            Category = category,
                            PreferenceScore = 1.0,
                            SearchCount = 1,
                            LastUpdated = DateTime.Now
                        };
                        _context.UserPreferences.Add(preference);
                    }
                    else
                    {
                        // Increase preference score based on search frequency and recency
                        var timeDecay = CalculateTimeDecay(preference.LastUpdated);
                        var frequencyBonus = Math.Min(preference.SearchCount * 0.1, 2.0); // Cap at 2.0
                        var recencyBonus = resultCount > 0 ? 0.5 : -0.2; // Bonus for successful searches
                        
                        preference.PreferenceScore = (preference.PreferenceScore * timeDecay) + 1.0 + frequencyBonus + recencyBonus;
                        preference.SearchCount++;
                        preference.LastUpdated = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user preferences: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets user preferences with calculated scores
        /// </summary>
        private async Task<LinkedList<UserPreference>> GetUserPreferencesAsync()
        {
            var preferences = await _context.UserPreferences.ToListAsync();
            
            // Apply time decay to all preferences
            foreach (var preference in preferences)
            {
                var timeDecay = CalculateTimeDecay(preference.LastUpdated);
                preference.PreferenceScore *= timeDecay;
            }

            // Convert to LinkedList for efficient processing
            var preferenceList = new LinkedList<UserPreference>();
            foreach (var preference in preferences.Where(p => p.PreferenceScore > 0.1))
            {
                preferenceList.AddLast(preference);
            }
            
            return preferenceList;
        }

        /// <summary>
        /// Gets popular events based on search frequency
        /// </summary>
        private async Task<Queue<Event>> GetPopularEventsAsync(int count)
        {
            var popularCategories = await _context.SearchHistories
                .Where(s => s.Category != null && s.ResultCount > 0)
                .GroupBy(s => s.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToListAsync();

            var recommendations = new Queue<Event>();

            foreach (var category in popularCategories)
            {
                var cat = (category.Category ?? string.Empty).ToLower();
                var events = await _context.Events
                    .Where(e => e.Category.ToLower() == cat)
                    .Where(e => e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .Take(count)
                    .ToListAsync();

                foreach (var eventItem in events)
                {
                    recommendations.Enqueue(eventItem);
                }
            }

            // If still not enough, get any upcoming events
            if (recommendations.Count < count)
            {
                var additionalEvents = await _context.Events
                    .Where(e => e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .Take(count - recommendations.Count)
                    .ToListAsync();

                foreach (var eventItem in additionalEvents)
                {
                    recommendations.Enqueue(eventItem);
                }
            }

            // Return only the requested count
            var result = new Queue<Event>();
            var currentCount = 0;
            while (recommendations.Count > 0 && currentCount < count)
            {
                result.Enqueue(recommendations.Dequeue());
                currentCount++;
            }
            return result;
        }

        /// <summary>
        /// Calculates time decay factor for preferences
        /// </summary>
        private double CalculateTimeDecay(DateTime lastUpdated)
        {
            var daysSinceUpdate = (DateTime.Now - lastUpdated).TotalDays;
            return Math.Exp(-daysSinceUpdate / 30.0); // 30-day half-life
        }

        /// <summary>
        /// Determines the type of search performed
        /// </summary>
        private string DetermineSearchType(string? searchTerm, string? category)
        {
            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(category))
                return "KeywordAndCategory";
            if (!string.IsNullOrEmpty(searchTerm))
                return "Keyword";
            if (!string.IsNullOrEmpty(category))
                return "Category";
            return "General";
        }

        /// <summary>
        /// Gets search analytics for admin purposes
        /// </summary>
        public async Task<SearchAnalyticsResult> GetSearchAnalyticsAsync()
        {
            var totalSearches = await _context.SearchHistories.CountAsync();
            var successfulSearches = await _context.SearchHistories.CountAsync(s => s.ResultCount > 0);
            var topCategories = await _context.SearchHistories
                .Where(s => s.Category != null)
                .GroupBy(s => s.Category!)
                .Select(g => new CategoryCount { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var recentSearches = await _context.SearchHistories
                .OrderByDescending(s => s.SearchDate)
                .Take(10)
                .ToListAsync();

            // Convert to advanced data structures
            var topCategoriesList = new LinkedList<CategoryCount>();
            foreach (var category in topCategories)
            {
                topCategoriesList.AddLast(category);
            }

            var recentSearchesQueue = new Queue<SearchHistory>();
            foreach (var search in recentSearches)
            {
                recentSearchesQueue.Enqueue(search);
            }

            return new SearchAnalyticsResult
            {
                TotalSearches = totalSearches,
                SuccessfulSearches = successfulSearches,
                SuccessRate = totalSearches > 0 ? (double)successfulSearches / totalSearches * 100 : 0,
                TopCategories = topCategoriesList,
                RecentSearches = recentSearchesQueue
            };
        }
    }
}
