using FoodLabelManager.API.DTOs;

namespace FoodLabelManager.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterRequest request);
        Task<AuthResponse> Login(LoginRequest request);
    }
}


