using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;

public class GetLeaveRequestDetailQueryHandler : IRequestHandler<GetLeaveRequestDetailQuery, LeaveRequestDetailsDTO>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetLeaveRequestDetailQueryHandler> _logger;

    public GetLeaveRequestDetailQueryHandler(ILeaveRequestRepository requestRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper, IAppLogger<GetLeaveRequestDetailQueryHandler> logger)
    {
        _requestRepository = requestRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<LeaveRequestDetailsDTO> Handle(GetLeaveRequestDetailQuery request, CancellationToken cancellationToken)
    {
        var leaveRequest = _mapper.Map<LeaveRequestDetailsDTO>(await _requestRepository.GetAsync(request.Id));

        if (leaveRequest == null)
        {
            throw new NotFoundException(nameof(LeaveRequest), request.Id);
        }
        
        // TODO: Add employee details as needed

        return leaveRequest;
    }
}