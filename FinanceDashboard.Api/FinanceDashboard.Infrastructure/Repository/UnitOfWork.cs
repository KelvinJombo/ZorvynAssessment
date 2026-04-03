using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Domain.Models;
using FinanceDashboard.Infrastructure.Context;

namespace FinanceDashboard.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DashboardDbContext _context;

        public IGenericRepository<FinancialRecord> FinancialRecordRepository { get; }

        public UnitOfWork(DashboardDbContext context)
        {
            _context = context;
            FinancialRecordRepository = new GenericRepository<FinancialRecord>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
