using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;
    private readonly IUserService _userService;

    public CreateLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper, IAppLogger<CreateLeaveRequestCommandHandler> logger, IUserService userService, ILeaveAllocationRepository leaveAllocationRepository)
    {
        _requestRepository = requestRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
        _userService = userService;
        _leaveAllocationRepository = leaveAllocationRepository;
    }

    public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestCommandValidator(_typeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Any())
        {
            _logger.LogWarning("Invalid Leave Request create: {1}", request);
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }
        
        // TODO: Get requesting employee's id
        // Check employee's allocation
        // if allocations aren't enough, return validation with error message
        var employeeId = _userService.UserId;
        
        var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);
        
        // if allocations aren't enough, return validation error with message
        if (allocation is null)
        {
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                "You do not have any allocations for this leave type."));
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        var daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
        if (daysRequested > allocation.NumberOfDays)
        {
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                nameof(request.EndDate), "You do not have enough days for this request"));
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
        leaveRequest.RequestingEmployeeId = employeeId;
        leaveRequest.DateRequested = DateTime.Now;
        await _requestRepository.CreateAsync(leaveRequest);

        return Unit.Value;

    }
}