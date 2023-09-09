using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
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
    private readonly IUserService _userService;


    public CreateLeaveAllocationCommandHandler(IMapper mapper, ILeaveAllocationRepository allocationRepository,
        ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveAllocationCommandHandler> logger,
        IUserService userService)
    {
        _mapper = mapper;
        _allocationRepository = allocationRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _logger = logger;
        _userService = userService;
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
        var employees = await _userService.GetEmployees();
        
        // Get Period
        var period = DateTime.Now.Year;

        var allocations = new List<Domain.LeaveAllocation>();
        
        foreach (var employee in employees)
        {
            var isAllocationExists =
                await _allocationRepository.AllocationExists(employee.Id, request.LeaveTypeId, period);

            if (!isAllocationExists)
            {
                allocations.Add(new Domain.LeaveAllocation()
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveTypes.Id,
                    NumberOfDays = leaveTypes.DefaultDays,
                    Period = period
                });
            }
            
        }

        if (allocations.Any())
        {
            await _allocationRepository.AddAllocations(allocations);
        }

        return Unit.Value;
    }
}