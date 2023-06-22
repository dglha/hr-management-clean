using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class LeaveTypeDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DefaultDays { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
