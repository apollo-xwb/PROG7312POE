using System;

namespace MunicipalServicesApp
{
    /// <summary>
    /// Represents a municipal service issue report
    /// </summary>
    public class Issue
    {
        public string Location { get; set; }        // Address or area where issue occurred
        public string Category { get; set; }        // Type of service (Sanitation, Roads, Utilities)
        public string Description { get; set; }     // Detailed description of the issue
        public string AttachedFilePath { get; set; } // Optional file attachment path
    }
}


