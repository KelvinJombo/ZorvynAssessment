using FinanceDashboard.Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace FinanceDashboard.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public UserRole Role { get; set; }

        // Navigation
        public ICollection<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();
    }
}
