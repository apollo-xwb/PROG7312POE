/*
 * SearchEvents.cshtml.cs - Event Search Page Model
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: Advanced data structure conversion patterns and Queue implementation guidance provided by Claude (Anthropic, 2024).
 * My implementation: Core business logic, UI integration, and search functionality.
 * 
 * Prompt used: "grade this implemntation in reference to the rubric and grade me incase i missed something critical" - referring to converting List<T> to advanced data structures (LinkedList, Stack, Queue, etc.)
 * to comply with project requirements that prohibit the use of Lists and Arrays.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;

namespace MunicipalServicesApp.Pages
{
    /// <summary>
    /// Page model for the Search Events page, responsible for handling event search functionality.
    /// </summary>
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

        // Queue data structure for FIFO (First In, First Out) processing of search results
        public Queue<Event> FilteredEvents { get; set; } = new Queue<Event>();
        
        // LinkedList for recommendations allows efficient traversal and modification
        public LinkedList<Event> Recommendations { get; set; } = new LinkedList<Event>();
        
        // HashSet ensures unique categories with O(1) lookup performance
        public HashSet<string> Categories { get; set; } = new HashSet<string>();

        /// <summary>
        /// Handles GET requests to the Search Events page.
        /// Loads initial data including categories and recommendations.
        /// </summary>
        public void OnGet()
        {
            LoadData();
        }

        /// <summary>
        /// Handles POST requests for event search functionality.
        /// Uses Queue data structure for search results and LinkedList for recommendations.
        /// </summary>
        /// <returns>Page result with search results or error message</returns>
        public IActionResult OnPost()
        {
            try
            {
                // Perform search using EventManager which returns Queue<Event>
                var searchResults = _eventManager.SearchEvents(SearchKeyword, SearchCategory, StartDate, EndDate);
                FilteredEvents = new Queue<Event>();
                
                // Convert search results to Queue for FIFO processing
                foreach (var eventItem in searchResults)
                {
                    FilteredEvents.Enqueue(eventItem); // Queue.Enqueue for FIFO behavior
                }

                // Get recommendations and convert to LinkedList
                var recommendations = _eventManager.GetRecommendedEvents();
                Recommendations = new LinkedList<Event>();
                foreach (var eventItem in recommendations)
                {
                    Recommendations.AddLast(eventItem); // LinkedList.AddLast for efficient insertion
                }

                // Provide user feedback based on search results
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

        /// <summary>
        /// Loads initial data for the search page including categories and recommendations.
        /// </summary>
        private void LoadData()
        {
            // Get unique categories using HashSet for efficient lookup
            Categories = _eventManager.GetCategories();
            
            // Load recommendations using LinkedList for efficient traversal
            var recommendations = _eventManager.GetRecommendedEvents();
            Recommendations = new LinkedList<Event>();
            foreach (var eventItem in recommendations)
            {
                Recommendations.AddLast(eventItem); // Maintain the LinkedList structure
            }
        }
    }
}
