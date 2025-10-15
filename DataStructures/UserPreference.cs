using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.DataStructures
{
    public class UserPreference
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;
        
        [Display(Name = "Preference Score")]
        public double PreferenceScore { get; set; } = 0.0;
        
        [Display(Name = "Search Count")]
        public int SearchCount { get; set; } = 0;
        
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
