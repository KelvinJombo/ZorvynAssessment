using FinanceDashboard.Domain.Models;

namespace FinanceDashboard.Application.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<FinancialRecord> FinancialRecordRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
