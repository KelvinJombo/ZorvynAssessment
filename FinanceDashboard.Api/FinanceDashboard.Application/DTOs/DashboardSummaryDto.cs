namespace FinanceDashboard.Application.DTOs
{
    public class DashboardSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetBalance { get; set; }

        public Dictionary<string, decimal> CategoryTotals { get; set; }

        public List<FinancialRecordResponseDto> RecentTransactions { get; set; }
    }
}
