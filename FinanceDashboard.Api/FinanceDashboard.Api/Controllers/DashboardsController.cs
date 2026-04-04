using FinanceDashboard.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/records")]
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


    }
}
