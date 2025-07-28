using FoodLabelManager.API.DTOs;
using FoodLabelManager.API.Models;

namespace FoodLabelManager.API.Services
{
    public interface IFoodLabelService
    {
        Task<IEnumerable<FoodLabelDto>> GetAllFoodLabels();
        Task<FoodLabelDto> GetFoodLabelById(int id);
        Task<FoodLabelDto> CreateFoodLabel(CreateFoodLabelDto foodLabelDto);
        Task<FoodLabelDto> UpdateFoodLabel(UpdateFoodLabelDto foodLabelDto);
        Task<bool> DeleteFoodLabel(int id);
    }
}


