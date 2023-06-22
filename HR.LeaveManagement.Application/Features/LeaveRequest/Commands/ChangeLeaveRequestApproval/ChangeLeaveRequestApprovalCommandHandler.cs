using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;

public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<ChangeLeaveRequestApprovalCommandHandler> _logger;

    public ChangeLeaveRequestApprovalCommandHandler(ILeaveRequestRepository requestRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper,
        IAppLogger<ChangeLeaveRequestApprovalCommandHandler> logger)
    {
        _requestRepository = requestRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _requestRepository.GetAsync(request.Id);

        if (leaveRequest is null) throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.Approved = request.Approved;
        await _requestRepository.UpdateAsync(leaveRequest);
        
        // if request is approved, get and update the employee's allocations

        return Unit.Value;
    }
}