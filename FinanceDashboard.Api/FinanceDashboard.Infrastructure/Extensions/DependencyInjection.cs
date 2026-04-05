using FinanceDashboard.Application.Interfaces.Repository;
using FinanceDashboard.Infrastructure.BackgroundJobs;
using FinanceDashboard.Infrastructure.Context;
using FinanceDashboard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceDashboard.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DashboardDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DashboardConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHostedService<UserInactivityService>();

            return services;
        }
    }
}
