using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;

namespace MunicipalServicesApp.Pages
{
    public class SearchEventsModel : PageModel
    {
        private readonly EventManager _eventManager;

        public SearchEventsModel(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        [BindProperty]
        public string? SearchKeyword { get; set; }

        [BindProperty]
        public string? SearchCategory { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        public List<Event> FilteredEvents { get; set; } = new List<Event>();
        public List<Event> Recommendations { get; set; } = new List<Event>();
        public HashSet<string> Categories { get; set; } = new HashSet<string>();

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPost()
        {
            try
            {
                // Debug: Log search parameters
                TempData["Debug"] = $"Search called with: Keyword='{SearchKeyword}', Category='{SearchCategory}', StartDate={StartDate}, EndDate={EndDate}";
                
                FilteredEvents = _eventManager.SearchEvents(SearchKeyword, SearchCategory, StartDate, EndDate);

                // Get recommendations after search
                Recommendations = _eventManager.GetRecommendedEvents();

                if (!FilteredEvents.Any())
                {
                    TempData["Info"] = "No events found matching your search criteria.";
                }
                else
                {
                    TempData["Success"] = $"Found {FilteredEvents.Count} event(s) matching your search.";
                }

                LoadData();
                return Page();
            }
            catch (Exception ex)
            {
                LoadData();
                TempData["Error"] = $"An error occurred during search: {ex.Message}";
                return Page();
            }
        }

        private void LoadData()
        {
            Categories = _eventManager.GetCategories();
            // Load recommendations on initial page load
            Recommendations = _eventManager.GetRecommendedEvents();
        }
    }
}
