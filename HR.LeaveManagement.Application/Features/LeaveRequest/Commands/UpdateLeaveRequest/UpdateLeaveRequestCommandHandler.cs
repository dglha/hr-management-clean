using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<UpdateLeaveAllocationCommandHandler> _logger;

    public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper, IAppLogger<UpdateLeaveAllocationCommandHandler> logger)
    {
        _requestRepository = requestRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _requestRepository.GetAsync(request.Id);
        
        if (leaveRequest == null) throw new NotFoundException(nameof(LeaveRequest), request.Id);
        
        var validator = new UpdateLeaveRequestCommandValidator(_requestRepository, _typeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Any())
            throw new BadRequestException("Invalid Leave Request", validationResult);

        _mapper.Map(request, leaveRequest);

        await _requestRepository.UpdateAsync(leaveRequest);

        return Unit.Value;
    }
}