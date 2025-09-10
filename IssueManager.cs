using System;
using System.Collections.Generic;

namespace MunicipalServicesApp
{
    /// <summary>
    /// Manages issue storage and gamification points
    /// </summary>
    public static class IssueManager
    {
        // In-memory storage for reported issues
        public static readonly List<Issue> ReportedIssues = new List<Issue>();
        // Gamification points counter
        public static int Points { get; private set; }

        /// <summary>
        /// Adds a new issue to the collection
        /// </summary>
        public static void AddIssue(Issue issue)
        {
            if (issue == null)
            {
                throw new ArgumentNullException("issue");
            }

            ReportedIssues.Add(issue);
        }

        /// <summary>
        /// Adds points to the user's total score
        /// </summary>
        public static int AddPoints(int pointsToAdd)
        {
            if (pointsToAdd < 0)
            {
                pointsToAdd = 0; // Prevent negative points
            }

            Points += pointsToAdd;
            return Points;
        }
    }
}


