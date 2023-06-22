using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes
{
    // TODO: Ask what n why we use record type
    public record GetLeaveTypesQuery : IRequest<IEnumerable<LeaveTypeDTO>>;
}
