using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.DataStructures
{
    public class Event
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Event Title")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}
