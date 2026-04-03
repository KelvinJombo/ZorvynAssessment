using FinanceDashboard.Domain.Models;

namespace FinanceDashboard.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
