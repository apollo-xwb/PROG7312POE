/*
 * EventManager.cs - Event Management Service
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: LINQ query optimization and advanced data structure implementation guidance provided by AI assistant.
 * Student implementation: Core business logic, data management, and service integration.
 */
using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Managers
{
    public class EventManager
    {
        private readonly ApplicationDbContext _context;
        private readonly RecommendationService _recommendationService;

        public EventManager(ApplicationDbContext context, RecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
        }

        public async Task<SortedDictionary<DateTime, LinkedList<Event>>> GetAllEventsAsync()
        {
            var events = await _context.Events
                .OrderBy(e => e.Date)
                .ToListAsync();

            var eventsByDate = new SortedDictionary<DateTime, LinkedList<Event>>();
            foreach (var eventItem in events)
            {
                var date = eventItem.Date.Date;
                if (!eventsByDate.ContainsKey(date))
                {
                    eventsByDate[date] = new LinkedList<Event>();
                }
                eventsByDate[date].AddLast(eventItem);
            }

            return eventsByDate;
        }

        public SortedDictionary<DateTime, LinkedList<Event>> GetAllEvents()
        {
            return GetAllEventsAsync().Result;
        }

        public async Task<HashSet<string>> GetCategoriesAsync()
        {
            var categories = await _context.Events
                .Select(e => e.Category)
                .Distinct()
                .ToListAsync();
            
            return new HashSet<string>(categories);
        }

        public HashSet<string> GetCategories()
        {
            return GetCategoriesAsync().Result;
        }

        public async Task<Event> AddEventAsync(Event eventItem)
        {
            eventItem.Id = Guid.NewGuid().ToString();
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();
            return eventItem;
        }

        public Event AddEvent(Event eventItem)
        {
            return AddEventAsync(eventItem).Result;
        }

        public async Task<Queue<Event>> SearchEventsAsync(string? keyword, string? category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                var kw = keyword.ToLower();
                query = query.Where(e => e.Title.ToLower().Contains(kw) || e.Description.ToLower().Contains(kw));
            }

            if (!string.IsNullOrEmpty(category))
            {
                var cat = category.ToLower();
                query = query.Where(e => e.Category.ToLower() == cat);
            }

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            var results = await query.OrderBy(e => e.Date).ToListAsync();

            var eventQueue = new Queue<Event>();
            foreach (var eventItem in results)
            {
                eventQueue.Enqueue(eventItem);
            }

            await _recommendationService.UpdateUserPreferencesAsync(keyword, category, results.Count);
            return eventQueue;
        }

        public Queue<Event> SearchEvents(string? keyword, string? category, DateTime? startDate, DateTime? endDate)
        {
            return SearchEventsAsync(keyword, category, startDate, endDate).Result;
        }

        public async Task<LinkedList<Event>> GetRecommendationsAsync()
        {
            var recommendations = await _recommendationService.GetPersonalizedRecommendationsAsync();
            
            var recommendationList = new LinkedList<Event>();
            foreach (var eventItem in recommendations)
            {
                recommendationList.AddLast(eventItem);
            }
            
            return recommendationList;
        }

        public LinkedList<Event> GetRecommendedEvents()
        {
            return GetRecommendationsAsync().Result;
        }

        public async Task<SearchAnalyticsResult> GetSearchAnalyticsAsync()
        {
            return await _recommendationService.GetSearchAnalyticsAsync();
        }

        public async Task InitializeSampleDataAsync()
        {
            if (await _context.Events.AnyAsync())
            {
                return;
            }

            // Use LinkedList for sample data initialization to comply with advanced data structure requirements
            var sampleEvents = new LinkedList<Event>();
            var events = new[]
            {
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Community Meeting - Cape Town",
                    Date = new DateTime(2025, 10, 15),
                    Category = "Community",
                    Description = "Monthly community meeting to discuss local issues and municipal services"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Heritage Day Festival - Johannesburg",
                    Date = new DateTime(2025, 9, 24),
                    Category = "Culture",
                    Description = "Annual Heritage Day celebration with traditional food, music, and dance"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Road Maintenance Workshop - Durban",
                    Date = new DateTime(2025, 11, 5),
                    Category = "Infrastructure",
                    Description = "Public workshop on upcoming road maintenance projects in the city"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Youth Sports Tournament - Pretoria",
                    Date = new DateTime(2025, 10, 20),
                    Category = "Sports",
                    Description = "Annual youth football and netball tournament at local sports complex"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Environmental Clean-up Day - Port Elizabeth",
                    Date = new DateTime(2025, 10, 12),
                    Category = "Environment",
                    Description = "Community beach and park clean-up initiative"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Municipal Budget Consultation - Bloemfontein",
                    Date = new DateTime(2025, 11, 18),
                    Category = "Community",
                    Description = "Public consultation on the upcoming municipal budget for 2026"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Arts and Crafts Market - Stellenbosch",
                    Date = new DateTime(2025, 10, 25),
                    Category = "Culture",
                    Description = "Local artisans showcase their work at the monthly market"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Water Conservation Seminar - Kimberley",
                    Date = new DateTime(2025, 11, 8),
                    Category = "Environment",
                    Description = "Educational seminar on water conservation techniques for residents"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Senior Citizens Health Fair - East London",
                    Date = new DateTime(2025, 10, 30),
                    Category = "Health",
                    Description = "Free health screenings and wellness information for senior citizens"
                },
                new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Cycling Safety Campaign - George",
                    Date = new DateTime(2025, 11, 12),
                    Category = "Safety",
                    Description = "Promoting cycling safety and infrastructure improvements"
                }
            };

            // Populate LinkedList with sample events using AddLast for efficient insertion
            foreach (var eventItem in events)
            {
                sampleEvents.AddLast(eventItem); // LinkedList.AddLast maintains insertion order
            }

            // Add all events to the database context
            _context.Events.AddRange(sampleEvents);
            await _context.SaveChangesAsync();
        }
    }
}
