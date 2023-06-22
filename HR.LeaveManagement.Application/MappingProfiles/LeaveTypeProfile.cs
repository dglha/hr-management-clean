using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;

namespace HR.LeaveManagement.Application.MappingProfiles;

public class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveTypeDTO, LeaveType>().ReverseMap();
        CreateMap<LeaveType, LeaveTypeDetailDTO>();
        CreateMap<CreateLeaveTypeCommand, LeaveType>();
        CreateMap<UpdateLeaveTypeCommand, LeaveType>();
    }
}

public class LeaveAllocationProfile : Profile
{
    public LeaveAllocationProfile()
    {
        CreateMap<LeaveAllocationDTO, LeaveAllocation>().ReverseMap();
        CreateMap<LeaveAllocation, LeaveAllocationDetailsDTO>();
        CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
    }
}