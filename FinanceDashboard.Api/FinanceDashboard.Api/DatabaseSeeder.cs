namespace FinanceDashboard.Api
{
    using FinanceDashboard.Domain.Models;
    using Microsoft.AspNetCore.Identity;

    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            //Seed Roles
            string[] roles = { "Viewer", "Analyst", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Role '{role}' created");
                }
            }

            //Seed Users 
            await CreateUserIfNotExists(userManager, logger,
                email: "admin@financedashboard.com",
                username: "admin",
                password: "Admin@123",
                firstName: "System",
                lastName: "Admin",
                role: "Admin");

            await CreateUserIfNotExists(userManager, logger,
                email: "analyst@financedashboard.com",
                username: "analyst",
                password: "Analyst@123",
                firstName: "Financial",
                lastName: "Analyst",
                role: "Analyst");

            await CreateUserIfNotExists(userManager, logger,
                email: "viewer@financedashboard.com",
                username: "viewer",
                password: "Viewer@123",
                firstName: "Data",
                lastName: "Viewer",
                role: "Viewer");

            logger.LogInformation("Database seeding completed");
        }

        private static async Task CreateUserIfNotExists(
            UserManager<User> userManager,
            ILogger logger,
            string email,
            string username,
            string password,
            string firstName,
            string lastName,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    logger.LogError($"Failed to create {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    return;
                }

                logger.LogInformation($"User '{email}' created");
            }

            //Ensures role is assigned
            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
                logger.LogInformation($"Role '{role}' assigned to '{email}'");
            }
        }
    }
}
