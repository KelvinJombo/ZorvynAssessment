using FinanceDashboard.Domain.Models.Enums;

namespace FinanceDashboard.Application.DTOs.Record
{
    public class UpdateFinancialRecordDto
    {
        public decimal Amount { get; set; }
        public RecordType Type { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
