/*
 * Events.cshtml.cs - Events Management Page Model
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: Advanced data structure conversion patterns and LINQ optimization guidance provided by AI assistant.
 * Student implementation: Core business logic, UI integration, and data management.
 * 
 * Prompt used: "my linkedlist is causing errors once the report is issued. how do i fix this implementation?" - referring to converting List<T> to advanced data structures (LinkedList, Stack, Queue, etc.)
 * to comply with project requirements that prohibit the use of Lists and Arrays.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;

namespace MunicipalServicesApp.Pages
{
    /// <summary>
    /// Page model for the Events page, responsible for displaying all events and recommendations.
    /// </summary>
    public class EventsModel : PageModel
    {
        private readonly EventManager _eventManager;

        public EventsModel(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        // LinkedList for efficient insertion/deletion for dynamic event collections
        public LinkedList<Event> FilteredEvents { get; set; } = new LinkedList<Event>();
        
        // LinkedList for recommendations 
        public LinkedList<Event> Recommendations { get; set; } = new LinkedList<Event>();
        
        // HashSet ensures unique categories with O(1) lookup performance
        public HashSet<string> Categories { get; set; } = new HashSet<string>();

        /// <summary>
        /// Handles GET requests to the Events page.
        /// Loads all events and recommendations for display.
        /// </summary>
        public void OnGet()
        {
            LoadData();
        }


        private void LoadData()
        {
            // Get unique categories using HashSet for efficient lookup
            Categories = _eventManager.GetCategories();
            
            // Get events sorted by date using SortedDictionary
            var allEvents = _eventManager.GetAllEvents();
            FilteredEvents = new LinkedList<Event>();
            
            // Convert SortedDictionary values to LinkedList for display

            foreach (var dateGroup in allEvents.Values)
            {
                foreach (var eventItem in dateGroup)
                {
                    FilteredEvents.AddLast(eventItem); // LinkedList.AddLast for efficient insertion
                }
            }
            
            // Get personalized recommendations and convert to LinkedList
            var recommendations = _eventManager.GetRecommendedEvents();
            Recommendations = new LinkedList<Event>();
            foreach (var eventItem in recommendations)
            {
                Recommendations.AddLast(eventItem);
            }
        }
    }
}