using FinanceDashboard.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FinanceDashboard.Infrastructure.BackgroundJobs
{
    public class UserInactivityService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UserInactivityService> _logger;

        public UserInactivityService(IServiceScopeFactory scopeFactory, ILogger<UserInactivityService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Runs continuously
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckInactiveUsers();

                //Run once every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckInactiveUsers()
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            try
            {
                var users = await userManager.Users
                    .Include(u => u.FinancialRecords)
                    .ToListAsync();

                var thresholdDate = DateTime.UtcNow.AddMonths(-3);

                foreach (var user in users)
                {
                    var lastTransaction = user.FinancialRecords
                        .OrderByDescending(x => x.Date)
                        .FirstOrDefault();

                    if (lastTransaction != null && lastTransaction.Date < thresholdDate)
                    {
                        if (user.IsActive) // only update if needed
                        {
                            user.IsActive = false;
                            await userManager.UpdateAsync(user);

                            _logger.LogInformation($"User {user.UserName} marked as inactive");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking inactive users");
            }
        }
    }
}
