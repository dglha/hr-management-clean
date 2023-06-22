using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public class
    GetLeaveRequestListQueryHandler : IRequestHandler<GetLeaveRequestListQuery, IEnumerable<LeaveRequestListDTO>>
{
    private readonly ILeaveRequestRepository _requestRepository;
    private readonly ILeaveTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetLeaveRequestListQueryHandler> _logger;

    public GetLeaveRequestListQueryHandler(ILeaveRequestRepository requestRepository,
        ILeaveTypeRepository typeRepository, IMapper mapper, IAppLogger<GetLeaveRequestListQueryHandler> logger)
    {
        _requestRepository = requestRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<LeaveRequestListDTO>> Handle(GetLeaveRequestListQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: Check if is logged employee

        var leaveRequests = await _requestRepository.GetLeaveRequestsWithDetails();
        var requests = _mapper.Map<IEnumerable<LeaveRequestListDTO>>(leaveRequests);

        // TODO: Fill requests with employee information
        return requests;
    }
}