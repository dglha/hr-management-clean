using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public class GetLeaveAllocationDetailQuery : IRequest<LeaveAllocationDetailsDTO>
{
    public int Id { get; set; }
}