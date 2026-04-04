using FinanceDashboard.Application.DTOs.Dashboard;
using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceDashboard.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(string userId)
        {
            var records = await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var income = records.Where(x => x.Type == RecordType.Income).Sum(x => x.Amount);
            var expenses = records.Where(x => x.Type == RecordType.Expense).Sum(x => x.Amount);

            var categoryTotals = records
                .GroupBy(x => x.Category)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            var recent = records
                .OrderByDescending(x => x.Date)
                .Take(5)
                .Select(r => new FinancialRecordResponseDto
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Type = r.Type.ToString(),
                    Category = r.Category,
                    Date = r.Date,
                    Notes = r.Notes
                }).ToList();

            return new DashboardSummaryDto
            {
                TotalIncome = income,
                TotalExpenses = expenses,
                NetBalance = income - expenses,
                CategoryTotals = categoryTotals,
                RecentTransactions = recent
            };
        }
    }
}
