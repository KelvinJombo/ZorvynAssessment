using FinanceDashboard.Application.DTOs.Auth;
using FinanceDashboard.Commons.Utilities;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<Response<string>> RegisterAsync(RegisterDto dto);
        Task<Response<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<Response<string>> LogoutAsync(string userId);
    }
}
