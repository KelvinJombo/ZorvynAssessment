using FinanceDashboard.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardsController : ControllerBase
    {
       
        private readonly IDashboardService _dashboardService;

        public DashboardsController(IDashboardService dashboardService)
        {
           
            _dashboardService = dashboardService;
        }
        
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;       


        [HttpGet("summary")]
        [Authorize(Roles = "Analyst,Admin")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _dashboardService.GetSummaryAsync(GetUserId());
            return Ok(result);
        }


        [HttpGet("total-income")]
        public async Task<IActionResult> GetTotalIncome()
        {
            var result = await _dashboardService.GetTotalIncomeAsync(GetUserId());
            return Ok(result);
        }


        [HttpGet("total-expenses")]
        public async Task<IActionResult> GetTotalExpenses()
        {
            var result = await _dashboardService.GetTotalExpensesAsync(GetUserId());
            return Ok(result);
        }


        [HttpGet("category-totals")]
        public async Task<IActionResult> GetCategoryTotals()
        {
            var result = await _dashboardService.GetCategoryTotalsAsync(GetUserId());
            return Ok(result);
        }


        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent()
        {
            var result = await _dashboardService.GetRecentAsync(GetUserId());
            return Ok(result);
        }


        [HttpGet("monthly-trends")]
        public async Task<IActionResult> GetMonthlyTrends()
        {
            var result = await _dashboardService.GetMonthlyTrendsAsync(GetUserId());
            return Ok(result);
        }


    }
}
