using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    private readonly ILeaveTypeRepository _typeRepository;

    public CreateLeaveRequestCommandValidator(ILeaveTypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
        
        Include(new BaseLeaveRequestValidator(_typeRepository));
    }
}