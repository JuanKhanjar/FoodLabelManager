
using System.ComponentModel.DataAnnotations;

namespace FoodLabelManager.API.DTOs
{
    public class FoodLabelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public UserDto? CreatedByUser { get; set; }
        public UserDto? ModifiedByUser { get; set; }
        public List<FoodLabelTranslationDto> Translations { get; set; } = new List<FoodLabelTranslationDto>();
    }

    public class CreateFoodLabelDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(7)]
        public string? Color { get; set; }

        public List<CreateFoodLabelTranslationDto> Translations { get; set; } = new List<CreateFoodLabelTranslationDto>();
    }

    public class UpdateFoodLabelDto : CreateFoodLabelDto
    {
        public bool IsActive { get; set; }
    }

    public class FoodLabelTranslationDto
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateFoodLabelTranslationDto
    {
        [Required]
        [MaxLength(10)]
        public string LanguageCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }

    public class PagedResultDto<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}


using System.ComponentModel.DataAnnotations;

namespace FoodLabelManager.API.DTOs
{
    public class FoodLabelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
    }

    public class CreateFoodLabelDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        public string Category { get; set; }
        public string Language { get; set; }
    }

    public class UpdateFoodLabelDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        public string Category { get; set; }
        public string Language { get; set; }
    }
}


