using FinanceDashboard.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceDashboard.Infrastructure.Context
{
    public class DashboardDbContext : IdentityDbContext<User>
    {
        public DashboardDbContext(DbContextOptions<DashboardDbContext> options)
            : base(options)
        {
        }

        public DbSet<FinancialRecord> FinancialRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FinancialRecord>(entity =>
            {
                entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");

                entity.HasOne(x => x.User)
                      .WithMany(u => u.FinancialRecords)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

}
