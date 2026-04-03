using FinanceDashboard.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDashboard.Domain.Models
{
    public class FinancialRecord : BaseEntity
    {
        public decimal Amount { get; set; }

        public RecordType Type { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }       
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
