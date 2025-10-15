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
            
            // Show all events
            var allEvents = _eventManager.GetAllEvents();
            FilteredEvents = allEvents.Values.SelectMany(events => events).OrderBy(e => e.Date).ToList();
            
            // Get recommendations
            Recommendations = _eventManager.GetRecommendedEvents();
        }
    }
}