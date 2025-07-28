
using FoodLabelManager.API.Data;
using FoodLabelManager.API.DTOs;
using FoodLabelManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodLabelManager.API.Services
{
    public class FoodLabelService : IFoodLabelService
    {
        private readonly ApplicationDbContext _context;

        public FoodLabelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FoodLabelDto?> GetFoodLabelByIdAsync(int id)
        {
            var foodLabel = await _context.FoodLabels
                .Include(fl => fl.CreatedByUser)
                .Include(fl => fl.ModifiedByUser)
                .Include(fl => fl.Translations)
                .FirstOrDefaultAsync(fl => fl.Id == id);

            return foodLabel == null ? null : MapFoodLabelToDto(foodLabel);
        }

        public async Task<PagedResultDto<FoodLabelDto>> GetAllFoodLabelsAsync(string? category, string? search, bool includeInactive, int page, int pageSize)
        {
            var query = _context.FoodLabels
                .Include(fl => fl.CreatedByUser)
                .Include(fl => fl.ModifiedByUser)
                .Include(fl => fl.Translations)
                .AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(fl => fl.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(fl => fl.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(fl => fl.Name.Contains(search) || fl.Description.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var foodLabels = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResultDto<FoodLabelDto>
            {
                Data = foodLabels.Select(MapFoodLabelToDto).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<FoodLabelDto> CreateFoodLabelAsync(CreateFoodLabelDto model, int createdByUserId)
        {
            var foodLabel = new FoodLabel
            {
                Name = model.Name,
                Description = model.Description,
                Category = model.Category,
                ImageUrl = model.ImageUrl,
                Color = model.Color,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = createdByUserId,
                IsActive = true,
                Translations = model.Translations.Select(t => new FoodLabelTranslation
                {
                    LanguageCode = t.LanguageCode,
                    Name = t.Name,
                    Description = t.Description
                }).ToList()
            };

            _context.FoodLabels.Add(foodLabel);
            await _context.SaveChangesAsync();

            // Reload to include CreatedByUser and ModifiedByUser
            await _context.Entry(foodLabel).Reference(fl => fl.CreatedByUser).LoadAsync();

            return MapFoodLabelToDto(foodLabel);
        }

        public async Task<FoodLabelDto?> UpdateFoodLabelAsync(int id, UpdateFoodLabelDto model, int modifiedByUserId)
        {
            var foodLabel = await _context.FoodLabels
                .Include(fl => fl.Translations)
                .FirstOrDefaultAsync(fl => fl.Id == id);

            if (foodLabel == null)
            {
                return null;
            }

            foodLabel.Name = model.Name;
            foodLabel.Description = model.Description;
            foodLabel.Category = model.Category;
            foodLabel.ImageUrl = model.ImageUrl;
            foodLabel.Color = model.Color;
            foodLabel.IsActive = model.IsActive;
            foodLabel.ModifiedAt = DateTime.UtcNow;
            foodLabel.ModifiedByUserId = modifiedByUserId;

            // Update translations
            _context.FoodLabelTranslation.RemoveRange(foodLabel.Translations);
            foodLabel.Translations = model.Translations.Select(t => new FoodLabelTranslation
            {
                LanguageCode = t.LanguageCode,
                Name = t.Name,
                Description = t.Description
            }).ToList();

            await _context.SaveChangesAsync();

            // Reload to include CreatedByUser and ModifiedByUser
            await _context.Entry(foodLabel).Reference(fl => fl.CreatedByUser).LoadAsync();
            await _context.Entry(foodLabel).Reference(fl => fl.ModifiedByUser).LoadAsync();

            return MapFoodLabelToDto(foodLabel);
        }

        public async Task<bool> DeleteFoodLabelAsync(int id)
        {
            var foodLabel = await _context.FoodLabels.FindAsync(id);
            if (foodLabel == null)
            {
                return false;
            }

            foodLabel.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetAllCategoriesAsync()
        {
            return await _context.FoodLabels
                .Where(fl => !string.IsNullOrWhiteSpace(fl.Category))
                .Select(fl => fl.Category!)
                .Distinct()
                .ToListAsync();
        }

        private FoodLabelDto MapFoodLabelToDto(FoodLabel foodLabel)
        {
            return new FoodLabelDto
            {
                Id = foodLabel.Id,
                Name = foodLabel.Name,
                Description = foodLabel.Description,
                Category = foodLabel.Category,
                ImageUrl = foodLabel.ImageUrl,
                Color = foodLabel.Color,
                CreatedAt = foodLabel.CreatedAt,
                ModifiedAt = foodLabel.ModifiedAt,
                IsActive = foodLabel.IsActive,
                CreatedByUser = foodLabel.CreatedByUser != null ? new UserDto
                {
                    Id = foodLabel.CreatedByUser.Id,
                    Username = foodLabel.CreatedByUser.Username,
                    Email = foodLabel.CreatedByUser.Email,
                    FirstName = foodLabel.CreatedByUser.FirstName,
                    LastName = foodLabel.CreatedByUser.LastName,
                    Role = foodLabel.CreatedByUser.Role
                } : null,
                ModifiedByUser = foodLabel.ModifiedByUser != null ? new UserDto
                {
                    Id = foodLabel.ModifiedByUser.Id,
                    Username = foodLabel.ModifiedByUser.Username,
                    Email = foodLabel.ModifiedByUser.Email,
                    FirstName = foodLabel.ModifiedByUser.FirstName,
                    LastName = foodLabel.ModifiedByUser.LastName,
                    Role = foodLabel.ModifiedByUser.Role
                } : null,
                Translations = foodLabel.Translations.Select(t => new FoodLabelTranslationDto
                {
                    Id = t.Id,
                    LanguageCode = t.LanguageCode,
                    Name = t.Name,
                    Description = t.Description
                }).ToList()
            };
        }
    }
}


using FoodLabelManager.API.Data;
using FoodLabelManager.API.DTOs;
using FoodLabelManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodLabelManager.API.Services
{
    public class FoodLabelService : IFoodLabelService
    {
        private readonly ApplicationDbContext _context;

        public FoodLabelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodLabelDto>> GetAllFoodLabels()
        {
            return await _context.FoodLabels
                .Select(fl => new FoodLabelDto
                {
                    Id = fl.Id,
                    Name = fl.Name,
                    Description = fl.Description,
                    ImageUrl = fl.ImageUrl,
                    Category = fl.Category,
                    Language = fl.Language
                })
                .ToListAsync();
        }

        public async Task<FoodLabelDto> GetFoodLabelById(int id)
        {
            var foodLabel = await _context.FoodLabels.FindAsync(id);
            if (foodLabel == null)
            {
                return null;
            }

            return new FoodLabelDto
            {
                Id = foodLabel.Id,
                Name = foodLabel.Name,
                Description = foodLabel.Description,
                ImageUrl = foodLabel.ImageUrl,
                Category = foodLabel.Category,
                Language = foodLabel.Language
            };
        }

        public async Task<FoodLabelDto> CreateFoodLabel(CreateFoodLabelDto foodLabelDto)
        {
            var foodLabel = new FoodLabel
            {
                Name = foodLabelDto.Name,
                Description = foodLabelDto.Description,
                ImageUrl = foodLabelDto.ImageUrl,
                Category = foodLabelDto.Category,
                Language = foodLabelDto.Language
            };

            _context.FoodLabels.Add(foodLabel);
            await _context.SaveChangesAsync();

            return new FoodLabelDto
            {
                Id = foodLabel.Id,
                Name = foodLabel.Name,
                Description = foodLabel.Description,
                ImageUrl = foodLabel.ImageUrl,
                Category = foodLabel.Category,
                Language = foodLabel.Language
            };
        }

        public async Task<FoodLabelDto> UpdateFoodLabel(UpdateFoodLabelDto foodLabelDto)
        {
            var foodLabel = await _context.FoodLabels.FindAsync(foodLabelDto.Id);
            if (foodLabel == null)
            {
                return null;
            }

            foodLabel.Name = foodLabelDto.Name;
            foodLabel.Description = foodLabelDto.Description;
            foodLabel.ImageUrl = foodLabelDto.ImageUrl;
            foodLabel.Category = foodLabelDto.Category;
            foodLabel.Language = foodLabelDto.Language;

            _context.Entry(foodLabel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new FoodLabelDto
            {
                Id = foodLabel.Id,
                Name = foodLabel.Name,
                Description = foodLabel.Description,
                ImageUrl = foodLabel.ImageUrl,
                Category = foodLabel.Category,
                Language = foodLabel.Language
            };
        }

        public async Task<bool> DeleteFoodLabel(int id)
        {
            var foodLabel = await _context.FoodLabels.FindAsync(id);
            if (foodLabel == null)
            {
                return false;
            }

            _context.FoodLabels.Remove(foodLabel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


