/*
 * IssueManager.cs - Issue Management Service
 * 
 * References:
 * Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
 * Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.
 * Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/
 * 
 * AI Assistance: Stack data structure implementation and ordering logic guidance provided by Claude (Anthropic, 2024).
 * Student implementation: Core business logic, data persistence, and service integration.
 */

using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.DataStructures;

namespace MunicipalServicesApp.Managers
{
    public class IssueManager
    {
        private readonly ApplicationDbContext _context;

        public IssueManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stack<Issue>> GetAllIssuesAsync()
        {
            var issues = await _context.Issues
                .OrderByDescending(i => i.SubmittedDate)
                .ToListAsync();

            var issueStack = new Stack<Issue>();
            for (int i = issues.Count - 1; i >= 0; i--)
            {
                issueStack.Push(issues[i]);
            }

            return issueStack;
        }

        public Stack<Issue> GetAllIssues()
        {
            return GetAllIssuesAsync().Result;
        }

        public async Task<Issue> AddIssueAsync(Issue issue)
        {
            issue.Id = Guid.NewGuid().ToString();
            issue.SubmittedDate = DateTime.Now;
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issue;
        }

        public Issue AddIssue(Issue issue)
        {
            return AddIssueAsync(issue).Result;
        }

        public async Task InitializeSampleDataAsync()
        {
            if (await _context.Issues.AnyAsync())
            {
                return;
            }

            // Use Stack for sample data initialization to comply with advanced data structure requirements
            var sampleIssues = new Stack<Issue>();
            var issues = new[]
            {
                new Issue
                {
                    Id = Guid.NewGuid().ToString(),
                    Location = "Cape Town CBD",
                    Category = "Roads",
                    Description = "Large pothole on Long Street causing traffic delays",
                    SubmittedDate = DateTime.Now.AddDays(-2)
                },
                new Issue
                {
                    Id = Guid.NewGuid().ToString(),
                    Location = "Durban Central",
                    Category = "Sanitation",
                    Description = "Blocked storm drain on West Street causing flooding",
                    SubmittedDate = DateTime.Now.AddDays(-1)
                },
                new Issue
                {
                    Id = Guid.NewGuid().ToString(),
                    Location = "Johannesburg Sandton",
                    Category = "Utilities",
                    Description = "Street light not working on Rivonia Road",
                    SubmittedDate = DateTime.Now.AddHours(-12)
                },
                new Issue
                {
                    Id = Guid.NewGuid().ToString(),
                    Location = "Pretoria City Centre",
                    Category = "Roads",
                    Description = "Broken traffic light at Church Square intersection",
                    SubmittedDate = DateTime.Now.AddHours(-6)
                },
                new Issue
                {
                    Id = Guid.NewGuid().ToString(),
                    Location = "Port Elizabeth Central",
                    Category = "Sanitation",
                    Description = "Overflowing rubbish bins on Main Street",
                    SubmittedDate = DateTime.Now.AddHours(-3)
                }
            };

            // Populate Stack with sample issues using Push for LIFO behavior
            foreach (var issue in issues)
            {
                sampleIssues.Push(issue); // Stack.Push maintains LIFO order
            }

            // Add all issues to the database context
            _context.Issues.AddRange(sampleIssues);
            await _context.SaveChangesAsync();
        }
    }
}
