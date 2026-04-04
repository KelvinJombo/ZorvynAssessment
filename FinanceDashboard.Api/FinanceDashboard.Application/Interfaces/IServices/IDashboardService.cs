using FinanceDashboard.Application.DTOs.Dashboard;
using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Commons.Utilities;

namespace FinanceDashboard.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<Response<decimal>> GetTotalIncomeAsync(string userId);
        Task<Response<decimal>> GetTotalExpensesAsync(string userId);
        Task<Response<Dictionary<string, decimal>>> GetCategoryTotalsAsync(string userId);
        Task<Response<List<FinancialRecordResponseDto>>> GetRecentAsync(string userId);
        Task<Response<List<MonthlyTrendDto>>> GetMonthlyTrendsAsync(string userId);
        Task<Response<DashboardSummaryDto>> GetSummaryAsync(string userId);
    }
}
