using FinanceDashboard.Application.DTOs.Dashboard;
using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Commons.Utilities;
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

        public async Task<Response<DashboardSummaryDto>> GetSummaryAsync(string userId)
        {
            var query = _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId);

            var income = await query
                .Where(x => x.Type == RecordType.Income)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            var expenses = await query
                .Where(x => x.Type == RecordType.Expense)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            var categoryTotals = await query
                .GroupBy(x => x.Category)
                .Select(g => new { g.Key, Total = g.Sum(x => x.Amount) })
                .ToDictionaryAsync(x => x.Key, x => x.Total);

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

            var summary = new DashboardSummaryDto
            {
                TotalIncome = income,
                TotalExpenses = expenses,
                NetBalance = income - expenses,
                CategoryTotals = categoryTotals,
                RecentTransactions = recent
            };

            return Response<DashboardSummaryDto>.Success(
                summary,
                ResponseMessages.DashboardFetched
            );
        }


        public async Task<Response<decimal>> GetTotalIncomeAsync(string userId)
        {
            var total = await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId && x.Type == RecordType.Income)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            return Response<decimal>.Success(
                total,
                ResponseMessages.Success
            );
        }


        public async Task<Response<decimal>> GetTotalExpensesAsync(string userId)
        {
            var total = await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId && x.Type == RecordType.Expense)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            return Response<decimal>.Success(
                total,
                ResponseMessages.Success
            );
        }


        public async Task<Response<Dictionary<string, decimal>>> GetCategoryTotalsAsync(string userId)
        {
            var result = await _unitOfWork.FinancialRecordRepository
                .Query()
                .Where(x => x.UserId == userId)
                .GroupBy(x => x.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .ToDictionaryAsync(x => x.Category, x => x.Total);

            return Response<Dictionary<string, decimal>>.Success(
                result,
                ResponseMessages.Success
            );
        }


        public async Task<Response<List<FinancialRecordResponseDto>>> GetRecentAsync(string userId)
        {
            var result = await _unitOfWork.FinancialRecordRepository
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

            return Response<List<FinancialRecordResponseDto>>.Success(
                result,
                ResponseMessages.Success
            );
        }


        public async Task<Response<List<MonthlyTrendDto>>> GetMonthlyTrendsAsync(string userId)
        {
            var result = await _unitOfWork.FinancialRecordRepository
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

            return Response<List<MonthlyTrendDto>>.Success(
                result,
                ResponseMessages.TrendsFetched
            );
        }

    }
}
