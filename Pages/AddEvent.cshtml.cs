using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;

namespace MunicipalServicesApp.Pages
{
    public class AddEventModel : PageModel
    {
        private readonly EventManager _eventManager;

        public AddEventModel(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        [BindProperty]
        public Event NewEvent { get; set; } = new Event();

        public void OnGet()
        {
            // Initialize the form
        }

        public IActionResult OnPost()
        {
            // Debug: Log what we received
            TempData["Debug"] = $"OnPost called. NewEvent.Title: '{NewEvent?.Title}', NewEvent.Category: '{NewEvent?.Category}', NewEvent.Date: {NewEvent?.Date}, NewEvent.Description: '{NewEvent?.Description}', ModelState.IsValid: {ModelState.IsValid}";
            
            if (!ModelState.IsValid)
            {
                // Debug: Log ModelState errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorMessage = string.Join(", ", errors);
                TempData["Error"] = $"Please fill in all required fields. Errors: {errorMessage}";
                return Page();
            }

            try
            {
                _eventManager.AddEvent(NewEvent);
                TempData["Success"] = $"Event '{NewEvent.Title}' added successfully!";
                return RedirectToPage("/Events");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while adding the event: {ex.Message}";
                return Page();
            }
        }
    }
}
