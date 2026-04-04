using FinanceDashboard.Application.DTOs.Dashboard;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(string userId);
    }
}
