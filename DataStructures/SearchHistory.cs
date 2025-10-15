using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.DataStructures
{
    public class SearchHistory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Display(Name = "Search Term")]
        public string? SearchTerm { get; set; }
        
        [Display(Name = "Category")]
        public string? Category { get; set; }
        
        [Display(Name = "Search Date")]
        public DateTime SearchDate { get; set; } = DateTime.Now;
        
        [Display(Name = "Result Count")]
        public int ResultCount { get; set; }
        
        [Display(Name = "Search Type")]
        public string SearchType { get; set; } = "General"; // General, Category, Date, Keyword
    }
}
