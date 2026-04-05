using FinanceDashboard.Commons.Utilities;
using FinanceDashboard.Domain.Models.Enums;

namespace FinanceDashboard.Application.DTOs.Record
{
    public class FinancialRecordFilterDto : PaginationParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Category { get; set; }
        public RecordType? Type { get; set; }

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
