/*
Academic Integrity and References

AI assistance declaration:
- Portions of the following methods were drafted with assistance from an AI programming assistant and then reviewed, adapted and validated by the author:
  • GetPersonalizedRecommendationsAsync
  • UpdateUserPreferencesAsync
  • GetSearchAnalyticsAsync

References:
- Freeman, A. (n.d.) Pro ASP.NET Core Razor Pages in C#. New York: Apress.
- Smith, J.P. (n.d.) Entity Framework Core in Action. Shelter Island, NY: Manning.
- Microsoft (n.d.) ASP.NET Core Razor Pages. Available at: https://learn.microsoft.com/aspnet/core/razor-pages/?view=aspnetcore-8.0 (Accessed: 14 October 2025).
- Microsoft (n.d.) Entity Framework Core – SQLite provider. Available at: https://learn.microsoft.com/ef/core/providers/sqlite/?tabs=dotnet-core-cli (Accessed: 14 October 2025).
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
        public List<CategoryCount> TopCategories { get; set; } = new List<CategoryCount>();
        public List<SearchHistory> RecentSearches { get; set; } = new List<SearchHistory>();
    }

    public class RecommendationService
    {
        private readonly ApplicationDbContext _context;

        public RecommendationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Analyzes user search patterns and generates intelligent recommendations
        /// </summary>
        public async Task<List<Event>> GetPersonalizedRecommendationsAsync(int maxRecommendations = 3)
        {
            var recommendations = new List<Event>();

            try
            {
                // Get user preferences based on search history
                var userPreferences = await GetUserPreferencesAsync();
                
                if (!userPreferences.Any())
                {
                    // If no preferences, return popular events
                    return await GetPopularEventsAsync(maxRecommendations);
                }

                // Sort preferences by score (highest first)
                var topPreferences = userPreferences
                    .OrderByDescending(p => p.PreferenceScore)
                    .ThenByDescending(p => p.SearchCount)
                    .Take(3)
                    .ToList();

                // Get events for top preferred categories
                foreach (var preference in topPreferences)
                {
                    var prefCat = preference.Category.ToLower();
                    var categoryEvents = await _context.Events
                        .Where(e => e.Category.ToLower() == prefCat)
                        .Where(e => e.Date >= DateTime.Now) // Only future events
                        .OrderBy(e => e.Date)
                        .Take(maxRecommendations)
                        .ToListAsync();

                    recommendations.AddRange(categoryEvents);
                    
                    if (recommendations.Count >= maxRecommendations)
                        break;
                }

                // If we don't have enough recommendations, fill with popular events
                if (recommendations.Count < maxRecommendations)
                {
                    var popularEvents = await GetPopularEventsAsync(maxRecommendations - recommendations.Count);
                    recommendations.AddRange(popularEvents);
                }

                return recommendations.Take(maxRecommendations).ToList();
            }
            catch (Exception ex)
            {
                // Log error and return popular events as fallback
                Console.WriteLine($"Error generating recommendations: {ex.Message}");
                return await GetPopularEventsAsync(maxRecommendations);
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
        private async Task<List<UserPreference>> GetUserPreferencesAsync()
        {
            var preferences = await _context.UserPreferences.ToListAsync();
            
            // Apply time decay to all preferences
            foreach (var preference in preferences)
            {
                var timeDecay = CalculateTimeDecay(preference.LastUpdated);
                preference.PreferenceScore *= timeDecay;
            }

            return preferences.Where(p => p.PreferenceScore > 0.1).ToList(); // Filter out very low scores
        }

        /// <summary>
        /// Gets popular events based on search frequency
        /// </summary>
        private async Task<List<Event>> GetPopularEventsAsync(int count)
        {
            var popularCategories = await _context.SearchHistories
                .Where(s => s.Category != null && s.ResultCount > 0)
                .GroupBy(s => s.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToListAsync();

            var recommendations = new List<Event>();

            foreach (var category in popularCategories)
            {
                var cat = (category.Category ?? string.Empty).ToLower();
                var events = await _context.Events
                    .Where(e => e.Category.ToLower() == cat)
                    .Where(e => e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .Take(count)
                    .ToListAsync();

                recommendations.AddRange(events);
            }

            // If still not enough, get any upcoming events
            if (recommendations.Count < count)
            {
                var additionalEvents = await _context.Events
                    .Where(e => e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .Take(count - recommendations.Count)
                    .ToListAsync();

                recommendations.AddRange(additionalEvents);
            }

            return recommendations.Take(count).ToList();
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

            return new SearchAnalyticsResult
            {
                TotalSearches = totalSearches,
                SuccessfulSearches = successfulSearches,
                SuccessRate = totalSearches > 0 ? (double)successfulSearches / totalSearches * 100 : 0,
                TopCategories = topCategories,
                RecentSearches = recentSearches
            };
        }
    }
}
