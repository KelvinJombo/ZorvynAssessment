using FinanceDashboard.Application.DTOs.Record;
using FinanceDashboard.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/records")]
    [Authorize]
    public class FinancialRecordsController : ControllerBase
    {
        private readonly IFinancialRecordService _service;

        public FinancialRecordsController(IFinancialRecordService service)
        {
            _service = service;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        [Authorize(Roles = "Viewer,Analyst,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync(GetUserId());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateFinancialRecordDto dto)
        {
            var result = await _service.CreateAsync(GetUserId(), dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, UpdateFinancialRecordDto dto)
        {
            var result = await _service.UpdateAsync(id, GetUserId(), dto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id, GetUserId());
            return NoContent();
        }
    }
}
