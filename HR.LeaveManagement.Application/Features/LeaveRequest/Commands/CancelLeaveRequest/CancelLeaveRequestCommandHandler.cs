using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveAllocationRepository _allocationRepository;

    public CancelLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository, ILeaveAllocationRepository allocationRepository)
    {
        _requestRepository = requestRepository;
        _allocationRepository = allocationRepository;
    }

    public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _requestRepository.GetAsync(request.Id);

        if (leaveRequest is null) throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.Cancelled = true;
        await _requestRepository.UpdateAsync(leaveRequest);

        // If already approved, re-evaluate the employee's allocations for the leave type
        
        if (leaveRequest.Approved != true) return Unit.Value;
        
        var leaveDaysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
        var leaveAllocation =
            await _allocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId,
                leaveRequest.LeaveTypeId);
        leaveAllocation.NumberOfDays += leaveDaysRequested;

        return Unit.Value;
    }
}