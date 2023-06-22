using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public class GetLeaveRequestListQuery : IRequest<IEnumerable<LeaveRequestListDTO>>
{
}