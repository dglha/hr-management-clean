using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public class
    GetLeaveAllocationDetailRequestHandler : IRequestHandler<GetLeaveAllocationDetailQuery, LeaveAllocationDetailsDTO>
{
    private readonly ILeaveAllocationRepository _allocationRepository;
    private readonly IMapper _mapper;

    public GetLeaveAllocationDetailRequestHandler(ILeaveAllocationRepository allocationRepository, IMapper mapper)
    {
        _allocationRepository = allocationRepository;
        _mapper = mapper;
    }

    public async Task<LeaveAllocationDetailsDTO> Handle(GetLeaveAllocationDetailQuery request,
        CancellationToken cancellationToken)
    {
        var leaveAllocation = await _allocationRepository.GetLeaveAllocationWithDetails(request.Id);
        if (leaveAllocation == null)
        {
            throw new NotFoundException(nameof(LeaveAllocation), request.Id);
        }

        return _mapper.Map<LeaveAllocationDetailsDTO>(leaveAllocation);
    }
}