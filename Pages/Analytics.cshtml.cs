using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Pages
{
    public class AnalyticsModel : PageModel
    {
        private readonly EventManager _eventManager;

        public AnalyticsModel(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public int TotalSearches { get; set; }
        public int SuccessfulSearches { get; set; }
        public double SuccessRate { get; set; }
        public List<CategoryCount> TopCategories { get; set; } = new List<CategoryCount>();
        public List<SearchHistory> RecentSearches { get; set; } = new List<SearchHistory>();
        public List<Event> Recommendations { get; set; } = new List<Event>();

        public async Task OnGetAsync()
        {
            try
            {
                // Get search analytics
                var analytics = await _eventManager.GetSearchAnalyticsAsync();
                
                TotalSearches = analytics.TotalSearches;
                SuccessfulSearches = analytics.SuccessfulSearches;
                SuccessRate = analytics.SuccessRate;
                
                // Convert from LinkedList<CategoryCount> to List<CategoryCount>
                TopCategories = new List<CategoryCount>();
                foreach (var category in analytics.TopCategories)
                {
                    TopCategories.Add(category);
                }
                
                // Convert from Queue<SearchHistory> to List<SearchHistory>
                RecentSearches = new List<SearchHistory>();
                foreach (var search in analytics.RecentSearches)
                {
                    RecentSearches.Add(search);
                }

                // Get personalized recommendations - convert from LinkedList<Event>
                var recommendations = await _eventManager.GetRecommendationsAsync();
                Recommendations = new List<Event>();
                foreach (var eventItem in recommendations)
                {
                    Recommendations.Add(eventItem);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading analytics: {ex.Message}";
            }
        }
    }
}
