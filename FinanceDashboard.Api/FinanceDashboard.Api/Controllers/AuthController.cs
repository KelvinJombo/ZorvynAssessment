using FinanceDashboard.Application.DTOs.Auth;
using FinanceDashboard.Application.Interfaces;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [EnableRateLimiting("Strict")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        [EnableRateLimiting("Strict")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.LogoutAsync(userId!);

            return StatusCode(result.StatusCode, result);
        }
    }
}
