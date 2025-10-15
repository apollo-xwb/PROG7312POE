/*
 * ReportIssue.cshtml.cs - Issue Reporting Page Model
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: File validation logic and Stack data structure implementation guidance provided by Claude (Anthropic, 2024)
 * My implementation: Core business logic, UI integration, and error handling.
 * 
 * Prompt used: "my stack is causing errors once the report is issued. how do i fix this implementation? refer to the screenshot" - referring to converting List<T> to advanced data structures (LinkedList, Stack, Queue, etc.)
 * to comply with project requirements that prohibit the use of Lists and Arrays.
 */

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


        // Most recent issues appear at the top for use in Stack operations
        public Stack<Issue> RecentIssues { get; set; } = new Stack<Issue>();

        /// <summary>
        /// Loads recent issues using Stack data structure for LIFO processing.
        /// </summary>
        public void OnGet()
        {
            var allIssues = _issueManager.GetAllIssues();
            RecentIssues = new Stack<Issue>();
            
            // Convert Stack to Array to avoid consuming the original Stack

            var issuesArray = allIssues.ToArray();
            var count = Math.Min(10, issuesArray.Length);
            
            // Push issues to Stack in reverse order so most recent appear at top
            for (int i = 0; i < count; i++)
            {
                RecentIssues.Push(issuesArray[i]);
            }
        }

        public IActionResult OnPost()
        {
            if (AttachedFile != null && AttachedFile.Length > 0)
            {
                var validationResult = ValidateFile(AttachedFile);
                if (!validationResult.IsValid)
                {
                    LoadRecentIssues();
                    TempData["Error"] = validationResult.ErrorMessage;
                    return Page();
                }
            }

            if (!ModelState.IsValid)
            {
                LoadRecentIssues();
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorMessage = string.Join(", ", errors);
                TempData["Error"] = $"Please fill in all required fields. {errorMessage}";
                return Page();
            }

            try
            {
                var newIssue = new Issue
                {
                    Location = IssueInput.Location,
                    Category = IssueInput.Category,
                    Description = IssueInput.Description
                };

                if (AttachedFile != null && AttachedFile.Length > 0)
                {
                    newIssue.AttachedFilePath = AttachedFile.FileName;
                }

                var addedIssue = _issueManager.AddIssue(newIssue);
                
                var allIssues = _issueManager.GetAllIssues();
                var totalCount = allIssues.Count;
                
                TempData["Success"] = $"Issue reported successfully! Issue ID: {addedIssue.Id}. Total issues now: {totalCount}";
                TempData["PointsAwarded"] = "10";
                
                IssueInput = new IssueInputModel();
                AttachedFile = null;
                ModelState.Clear();
                
                LoadRecentIssues();
                return Page();
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while reporting the issue. Please try again.";
                LoadRecentIssues();
                return Page();
            }
        }

        /// <summary>
        /// Loads recent issues into a Stack data structure for LIFO processing.
        /// </summary>
        private void LoadRecentIssues()
        {
            var allIssues = _issueManager.GetAllIssues();
            RecentIssues = new Stack<Issue>();
            
            // This prevents modifying the original Stack while enabling iteration
            var issuesArray = allIssues.ToArray();
            var count = Math.Min(10, issuesArray.Length);
            
            // Push issues to new Stack maintaining LIFO order
            for (int i = 0; i < count; i++)
            {
                RecentIssues.Push(issuesArray[i]); // Stack.Push maintains LIFO behavior
            }
        }

        private FileValidationResult ValidateFile(IFormFile file)
        {
            const long maxFileSize = 10 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "File size exceeds the maximum limit of 10MB. Please choose a smaller file."
                };
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Invalid file type. Only images (JPG, PNG, GIF, BMP), PDF, and Word documents (DOC, DOCX) are allowed."
                };
            }

            var allowedMimeTypes = new[]
            {
                "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp",
                "application/pdf",
                "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Invalid file type detected. Please upload a valid image, PDF, or Word document."
                };
            }

            return new FileValidationResult { IsValid = true };
        }

        public class FileValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
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
