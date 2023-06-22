using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _allocationRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<UpdateLeaveAllocationCommandHandler> _logger;

    public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository allocationRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper, IAppLogger<UpdateLeaveAllocationCommandHandler> logger)
    {
        _allocationRepository = allocationRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationCommandValidator(_allocationRepository, _typeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid Leave Allocation", validationResult);
        }

        var leaveAllocation = await _allocationRepository.GetAsync(request.Id);

        if (leaveAllocation is null)
        {
            throw new NotFoundException(nameof(LeaveAllocation), request.Id);
        }

        _mapper.Map(request, leaveAllocation);

        await _allocationRepository.UpdateAsync(leaveAllocation);
        return Unit.Value;
    }
}