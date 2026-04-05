namespace FinanceDashboard.Commons.Utilities
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public int Skip => (PageNumber - 1) * PageSize;
    }
}
