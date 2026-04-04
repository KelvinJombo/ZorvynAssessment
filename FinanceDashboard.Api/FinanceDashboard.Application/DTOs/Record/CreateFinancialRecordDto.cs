namespace FinanceDashboard.Application.DTOs.Record
{
    public class CreateFinancialRecordDto
    {
        public decimal Amount { get; set; }
        public string Type { get; set; } // "Income" or "Expense"
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
