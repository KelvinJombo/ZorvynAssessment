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
            var query = _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId);

            //Database-side aggregations
            var income = await query
                .Where(x => x.Type == RecordType.Income)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            var expenses = await query
                .Where(x => x.Type == RecordType.Expense)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            //Category totals. DB-side calculation
            var categoryTotals = await query
                .GroupBy(x => x.Category)
                .Select(g => new { g.Key, Total = g.Sum(x => x.Amount) })
                .ToDictionaryAsync(x => x.Key, x => x.Total);

            //Recent 5 transactions 
            var recent = await query
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
                })
                .ToListAsync();

            return new DashboardSummaryDto
            {
                TotalIncome = income,
                TotalExpenses = expenses,
                NetBalance = income - expenses,
                CategoryTotals = categoryTotals,
                RecentTransactions = recent
            };
        }


        public async Task<decimal> GetTotalIncomeAsync(string userId)
        {
            return await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId && x.Type == RecordType.Income)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;
        }


        public async Task<decimal> GetTotalExpensesAsync(string userId)
        {
            return await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId && x.Type == RecordType.Expense)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;
        }


        public async Task<Dictionary<string, decimal>> GetCategoryTotalsAsync(string userId)
        {
            return await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
                .GroupBy(x => x.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .ToDictionaryAsync(x => x.Category, x => x.Total);
        }


        public async Task<List<FinancialRecordResponseDto>> GetRecentAsync(string userId)
        {
            return await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
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
                })
                .ToListAsync();
        }


        public async Task<List<MonthlyTrendDto>> GetMonthlyTrendsAsync(string userId)
        {
            return await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
                .GroupBy(x => new { x.Date.Year, x.Date.Month })
                .Select(g => new MonthlyTrendDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Income = g
                        .Where(x => x.Type == RecordType.Income)
                        .Sum(x => (decimal?)x.Amount) ?? 0,
                    Expense = g
                        .Where(x => x.Type == RecordType.Expense)
                        .Sum(x => (decimal?)x.Amount) ?? 0
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
        }

    }
}
