using FinanceDashboard.Application.DTOs.Auth;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task LogoutAsync(string userId);
    }
}
