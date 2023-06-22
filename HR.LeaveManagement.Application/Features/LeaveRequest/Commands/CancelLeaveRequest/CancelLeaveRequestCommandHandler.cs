using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;

    public CancelLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _requestRepository.GetAsync(request.Id);

        if (leaveRequest is null) throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.Cancelled = true;
        
        
        // TODO: If already approved, re-evaluate the employee's allocations for the leave type

        await _requestRepository.UpdateAsync(leaveRequest);

        return Unit.Value;
    }
}