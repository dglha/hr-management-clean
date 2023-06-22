using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;

public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;

    public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _requestRepository.GetAsync(request.Id);

        if (leaveRequest == null) throw new NotFoundException(nameof(LeaveRequest), request.Id);

        await _requestRepository.DeleteAsync(leaveRequest);

        return Unit.Value;
    }
}