/*
 * Analytics.cshtml.cs - Analytics and Recommendations Page Model
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: Advanced data structure conversion patterns and analytics implementation guidance provided by Claude (Anthropic, 2024).
 * My implementation: Core business logic, UI integration, and analytics functionality.
 * 
 * Prompt used: "can you suggest a more efficient way to implement this? the method i used is not efficient and it causes errors when running" - referring to converting List<T> to advanced data structures (LinkedList, Stack, Queue, etc.)
 * to comply with project requirements that prohibit the use of Lists and Arrays.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Pages
{
    /// <summary>
    /// Page model for the Analytics page, responsible for displaying search analytics and recommendations.
    /// Uses LinkedList for categories and recommendations, Queue for search history
    /// </summary>
    public class AnalyticsModel : PageModel
    {
        private readonly EventManager _eventManager;

        public AnalyticsModel(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        // Analytics properties for displaying search statistics
        public int TotalSearches { get; set; }
        public int SuccessfulSearches { get; set; }
        public double SuccessRate { get; set; }
        
        // LinkedList for top categories allows efficient traversal and modification
        public LinkedList<CategoryCount> TopCategories { get; set; } = new LinkedList<CategoryCount>();
        
        // Queue for recent searches demonstrates FIFO processing of search history
        public Queue<SearchHistory> RecentSearches { get; set; } = new Queue<SearchHistory>();
        
        // LinkedList for recommendations allows efficient traversal and modification
        public LinkedList<Event> Recommendations { get; set; } = new LinkedList<Event>();

        /// <summary>
        /// Handles GET requests to the Analytics page.
        /// Loads search analytics and recommendations using advanced data structures.
        /// </summary>
        public async Task OnGetAsync()
        {
            try
            {
                // Get search analytics from EventManager
                var analytics = await _eventManager.GetSearchAnalyticsAsync();
                
                // Set basic analytics properties
                TotalSearches = analytics.TotalSearches;
                SuccessfulSearches = analytics.SuccessfulSearches;
                SuccessRate = analytics.SuccessRate;
                
                // Convert top categories to LinkedList for efficient traversal
                TopCategories = new LinkedList<CategoryCount>();
                foreach (var category in analytics.TopCategories)
                {
                    TopCategories.AddLast(category); // LinkedList.AddLast for efficient insertion
                }
                
                // Convert recent searches to Queue for FIFO processing
                RecentSearches = new Queue<SearchHistory>();
                foreach (var search in analytics.RecentSearches)
                {
                    RecentSearches.Enqueue(search); // Queue.Enqueue for FIFO behavior
                }

                // Get personalized recommendations and convert to LinkedList
                var recommendations = await _eventManager.GetRecommendationsAsync();
                Recommendations = new LinkedList<Event>();
                foreach (var eventItem in recommendations)
                {
                    Recommendations.AddLast(eventItem); // Maintain LinkedList structure
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading analytics: {ex.Message}";
            }
        }
    }
}
