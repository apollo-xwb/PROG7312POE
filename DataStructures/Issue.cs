using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.DataStructures
{
    public class Issue
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        
        [Display(Name = "Attached File")]
        public string? AttachedFilePath { get; set; }
        
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
    }
}
