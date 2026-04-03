using FinanceDashboard.Application.DTOs;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(string userId);
    }
}
