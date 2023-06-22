using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            CreateMap<LeaveRequestListDTO, LeaveRequest>().ReverseMap();
            CreateMap<LeaveRequestDetailsDTO, LeaveRequest>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestDetailsDTO>();
            CreateMap<CreateLeaveRequestCommand, LeaveRequest>();
            CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();
        }
    }
}
