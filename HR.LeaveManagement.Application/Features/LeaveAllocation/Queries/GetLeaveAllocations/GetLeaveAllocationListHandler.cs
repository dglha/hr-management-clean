using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;

public class GetLeaveAllocationListHandler : IRequestHandler<GetLeaveAllocationListQuery, IEnumerable<LeaveAllocationDTO>>
{
    private readonly ILeaveAllocationRepository _allocationRepository;
    private readonly IMapper _mapper;

    public GetLeaveAllocationListHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
    {
        _allocationRepository = leaveAllocationRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<LeaveAllocationDTO>> Handle(GetLeaveAllocationListQuery request, CancellationToken cancellationToken)
    {
        // TODO: - Get records for specific user
        // TODO: - Get allocations per employee

        var leaveAllocations = await _allocationRepository.GetLeaveAllocationsWithDetails();
        var allocations = _mapper.Map<IEnumerable<LeaveAllocationDTO>>(leaveAllocations);

        return allocations;
    }
}