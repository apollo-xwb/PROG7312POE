using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Managers;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Pages
{
    public class ReportIssueModel : PageModel
    {
        private readonly IssueManager _issueManager;

        public ReportIssueModel(IssueManager issueManager)
        {
            _issueManager = issueManager;
        }

        [BindProperty]
        public IssueInputModel IssueInput { get; set; } = new IssueInputModel();

        [BindProperty]
        public IFormFile? AttachedFile { get; set; }

        public List<Issue> RecentIssues { get; set; } = new List<Issue>();

        public void OnGet()
        {
            // Convert from Stack<Issue> to List<Issue>
            var allIssues = _issueManager.GetAllIssues();
            RecentIssues = new List<Issue>();
            var count = 0;
            while (allIssues.Count > 0 && count < 5)
            {
                RecentIssues.Add(allIssues.Pop());
                count++;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Convert from Stack<Issue> to List<Issue>
                var allIssues = _issueManager.GetAllIssues();
                RecentIssues = new List<Issue>();
                var count = 0;
                while (allIssues.Count > 0 && count < 5)
                {
                    RecentIssues.Add(allIssues.Pop());
                    count++;
                }
                
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorMessage = string.Join(", ", errors);
                TempData["Error"] = $"Please fill in all required fields. {errorMessage}";
                
                return Page();
            }

            try
            {
                // Create a new Issue from the input model
                var newIssue = new Issue
                {
                    Location = IssueInput.Location,
                    Category = IssueInput.Category,
                    Description = IssueInput.Description
                };

                // Handle file upload if present
                if (AttachedFile != null && AttachedFile.Length > 0)
                {
                    // For demo purposes, just store the filename
                    // In a real application, you would save the file to a secure location
                    newIssue.AttachedFilePath = AttachedFile.FileName;
                }

                var addedIssue = _issueManager.AddIssue(newIssue);
                
                // Get total count from Stack
                var allIssues = _issueManager.GetAllIssues();
                var totalCount = allIssues.Count;
                
                TempData["Success"] = $"Issue reported successfully! Issue ID: {addedIssue.Id}. Total issues now: {totalCount}";
                TempData["PointsAwarded"] = "10";
                
                // Clear the form
                IssueInput = new IssueInputModel();
                AttachedFile = null;
                ModelState.Clear();
                
                // Refresh recent issues - convert from Stack<Issue> to List<Issue>
                var refreshIssues = _issueManager.GetAllIssues();
                RecentIssues = new List<Issue>();
                var count = 0;
                while (refreshIssues.Count > 0 && count < 5)
                {
                    RecentIssues.Add(refreshIssues.Pop());
                    count++;
                }
                
                
                return Page();
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while reporting the issue. Please try again.";
                
                // Convert from Stack<Issue> to List<Issue>
                var allIssues = _issueManager.GetAllIssues();
                RecentIssues = new List<Issue>();
                var count = 0;
                while (allIssues.Count > 0 && count < 5)
                {
                    RecentIssues.Add(allIssues.Pop());
                    count++;
                }
                
                return Page();
            }
        }

        public class IssueInputModel
        {
            [Required(ErrorMessage = "Location is required")]
            [Display(Name = "Location")]
            public string Location { get; set; } = string.Empty;
            
            [Required(ErrorMessage = "Category is required")]
            [Display(Name = "Category")]
            public string Category { get; set; } = string.Empty;
            
            [Required(ErrorMessage = "Description is required")]
            [Display(Name = "Description")]
            public string Description { get; set; } = string.Empty;
        }
    }
}
