using FinanceDashboard.Application.DTOs.Dashboard;
using FinanceDashboard.Application.DTOs.Record;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(string userId);
        Task<decimal> GetTotalIncomeAsync(string userId);
        Task<decimal> GetTotalExpensesAsync(string userId);
        Task<Dictionary<string, decimal>> GetCategoryTotalsAsync(string userId);
        Task<List<FinancialRecordResponseDto>> GetRecentAsync(string userId);
        Task<List<MonthlyTrendDto>> GetMonthlyTrendsAsync(string userId);
    }
}
