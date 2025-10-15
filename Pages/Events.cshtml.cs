using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;

namespace MunicipalServicesApp.Pages
{
    public class EventsModel : PageModel
    {
        private readonly EventManager _eventManager;

        public EventsModel(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public List<Event> FilteredEvents { get; set; } = new List<Event>();
        public List<Event> Recommendations { get; set; } = new List<Event>();
        public HashSet<string> Categories { get; set; } = new HashSet<string>();

        public void OnGet()
        {
            LoadData();
        }

        private void LoadData()
        {
            Categories = _eventManager.GetCategories();
            
            // Show all events - convert from SortedDictionary<DateTime, LinkedList<Event>>
            var allEvents = _eventManager.GetAllEvents();
            FilteredEvents = new List<Event>();
            foreach (var dateGroup in allEvents.Values)
            {
                foreach (var eventItem in dateGroup)
                {
                    FilteredEvents.Add(eventItem);
                }
            }
            FilteredEvents = FilteredEvents.OrderBy(e => e.Date).ToList();
            
            // Get recommendations - convert from LinkedList<Event>
            var recommendations = _eventManager.GetRecommendedEvents();
            Recommendations = new List<Event>();
            foreach (var eventItem in recommendations)
            {
                Recommendations.Add(eventItem);
            }
        }
    }
}