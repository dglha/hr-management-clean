using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly ILeaveAllocationRepository _allocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<CreateLeaveAllocationCommandHandler> _logger;


    public CreateLeaveAllocationCommandHandler(IMapper mapper, ILeaveAllocationRepository allocationRepository, ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveAllocationCommandHandler> logger)
    {
        _mapper = mapper;
        _allocationRepository = allocationRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Any())
        {
            _logger.LogWarning("Error in create leave allocation");
            throw new BadRequestException("Invalid Leave Allocation request", validationResult);
        }
        
        // Get leave type for allocations
        var leaveTypes = await _leaveTypeRepository.GetAsync(request.LeaveTypeId);
        
        // Get Employee
        
        // Get Period
        
        // Assign Allocations
        var leaveAllocation = _mapper.Map<Domain.LeaveAllocation>(request);
        await _allocationRepository.CreateAsync(leaveAllocation);
        return Unit.Value;
    }
}