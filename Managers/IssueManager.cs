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

            // Convert to Stack for LIFO processing (most recent first)
            var issueStack = new Stack<Issue>();
            foreach (var issue in issues)
            {
                issueStack.Push(issue);
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
            // Check if data already exists
            if (await _context.Issues.AnyAsync())
            {
                return; // Data already exists
            }

            var sampleIssues = new List<Issue>
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

            _context.Issues.AddRange(sampleIssues);
            await _context.SaveChangesAsync();
        }
    }
}
