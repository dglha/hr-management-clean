using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class GetLeaveTypesDetailsQueryHandler : IRequestHandler<GetLeaveTypesDetailsQuery, LeaveTypeDetailDTO>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public GetLeaveTypesDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }
        public async Task<LeaveTypeDetailDTO> Handle(GetLeaveTypesDetailsQuery request, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepository.GetAsync(request.Id);
            var data = _mapper.Map<LeaveTypeDetailDTO>(leaveType);

            return data;
        }
    }
}
