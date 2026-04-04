using FinanceDashboard.Application.DTOs.Auth;
using FinanceDashboard.Application.Interfaces;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Domain.Models;
using FinanceDashboard.Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceDashboard.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            UserManager<User> userManager,
            IJwtTokenService jwtService,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = dto.Role,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));


            //Role Assignment to New Users
            var roleName = user.Role.ToString(); 
            await _userManager.AddToRoleAsync(user, roleName);

            return "User registered successfully";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.FinancialRecords)
                .FirstOrDefaultAsync(u => u.UserName == dto.Username);

            if (user == null)
                throw new Exception("Invalid credentials");

            //Auto deactivate if inactive for 3 months
            var lastTransaction = user.FinancialRecords
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();

            if (lastTransaction != null && lastTransaction.Date < DateTime.UtcNow.AddMonths(-3))
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }

            if (!user.IsActive)
                throw new Exception("Account is inactive");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                throw new Exception("Invalid credentials");

            return _jwtService.GenerateToken(user);
        }

        public async Task LogoutAsync(string userId)
        {
            // Stateless JWT → no real logout unless you track tokens
            // Here we simulate logout by updating security stamp

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            await _userManager.UpdateSecurityStampAsync(user);
        }
    }
}
