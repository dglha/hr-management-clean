using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;

    public CreateLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper, IAppLogger<CreateLeaveRequestCommandHandler> logger)
    {
        _requestRepository = requestRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
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

        var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
        await _requestRepository.CreateAsync(leaveRequest);

        return Unit.Value;

    }
}