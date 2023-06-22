using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _requestRepository;

    public UpdateLeaveRequestCommandValidator(ILeaveRequestRepository requestRepository, ILeaveTypeRepository typeRepository)
    {
        _requestRepository = requestRepository;
        
        Include(new BaseLeaveRequestValidator(typeRepository));

        RuleFor(p => p.Id)
            .NotNull()
            .MustAsync(LeaveRequestMustExist)
            .WithMessage("{PropertyName} must be present");
    }

    private async Task<bool> LeaveRequestMustExist(int id, CancellationToken arg2)
    {
        return await _requestRepository.IsLeaveRequestExist(id);
    }
}
