using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;

public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _allocationRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<DeleteLeaveAllocationCommandHandler> _logger;

    public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository allocationRepository, IMapper mapper,
        IAppLogger<DeleteLeaveAllocationCommandHandler> logger)
    {
        _allocationRepository = allocationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var leaveAllocation = await _allocationRepository.GetAsync(request.Id);

        if (leaveAllocation == null)
        {
            throw new NotFoundException(nameof(LeaveAllocation), request.Id);
        }

        await _allocationRepository.DeleteAsync(leaveAllocation);
        return Unit.Value;
    }
}