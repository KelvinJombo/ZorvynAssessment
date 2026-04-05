using FinanceDashboard.Application.DTOs.Auth;
using FinanceDashboard.Application.Interfaces;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Commons.Utilities;
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

        public async Task<Response<string>> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.Username);

            if (existingUser != null)
                return Response<string>.Failure(
                    ResponseMessages.UserAlreadyExists,
                    StatusCodes.BadRequest
                );

            var emailExists = await _userManager.FindByEmailAsync(dto.Email);

            if (emailExists != null)
                return Response<string>.Failure(
                    ResponseMessages.EmailAlreadyExists,
                    StatusCodes.BadRequest
                );

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
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                return Response<string>.Failure(
                    ResponseMessages.Failed,
                    StatusCodes.BadRequest,
                    errors
                );
            }

            // ✅ Assign role
            var roleName = user.Role.ToString();
            await _userManager.AddToRoleAsync(user, roleName);

            return Response<string>.Success(
                "User created",
                ResponseMessages.UserCreated,
                StatusCodes.Created
            );
        }

        public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.FinancialRecords)
                .FirstOrDefaultAsync(u => u.UserName == dto.Username);

            if (user == null)
                return Response<AuthResponseDto>.Failure(
                    ResponseMessages.InvalidCredentials,
                    StatusCodes.Unauthorized
                );            

            if (!user.IsActive)
                return Response<AuthResponseDto>.Failure(
                    ResponseMessages.AccountInactive,
                    StatusCodes.Unauthorized
                );

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                return Response<AuthResponseDto>.Failure(
                    ResponseMessages.InvalidCredentials,
                    StatusCodes.Unauthorized
                );

            var token = _jwtService.GenerateToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(24);
            var authResponse = new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,               
            };

            return Response<AuthResponseDto>.Success(
                authResponse,
                ResponseMessages.Success                
            );
        }

        public async Task<Response<string>> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Response<string>.Failure(
                    ResponseMessages.UserNotFound,
                    StatusCodes.NotFound
                );

            await _userManager.UpdateSecurityStampAsync(user);

            return Response<string>.Success(
                "Logged out",
                ResponseMessages.LogoutSuccess
            );
        }
    }
}
