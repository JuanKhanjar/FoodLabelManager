using System.ComponentModel.DataAnnotations;

namespace FoodLabelManager.API.Models
{
    public class FoodLabel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        // Optional: Add properties for categorization, e.g., type of food, dietary restrictions
        public string Category { get; set; }
        public string Language { get; set; } = "ar"; // Default to Arabic
    }
}


