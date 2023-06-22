using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;

public class GetLeaveRequestDetailQuery : IRequest<LeaveRequestDetailsDTO>
{
    public int Id { get; set; }
}