using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>

{
    private readonly ILeaveAllocationRepository _allocationRepository;
    private readonly ILeaveTypeRepository _typeRepository;

    public UpdateLeaveAllocationCommandValidator(ILeaveAllocationRepository allocationRepository,
        ILeaveTypeRepository typeRepository)
    {
        _allocationRepository = allocationRepository;
        _typeRepository = typeRepository;

        // Rule
        RuleFor(p => p.NumOfDays)
            .GreaterThan(0).WithMessage("{PropertyName} must greater than {ComparisonValue}");

        RuleFor(p => p.LeaveTypeId)
            .GreaterThan(0)
            .MustAsync(LeaveTypeMustExist)
            .WithMessage("{PropertyName} does not exist");

        RuleFor(p => p.Period)
            .GreaterThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("{PropertyName} must be after {ComparisonValue}");

        RuleFor(p => p.Id)
            .GreaterThan(0)
            .MustAsync(LeaveAllocationExist)
            .WithMessage("{PropertyName} does not exist");
    }

    private async Task<bool> LeaveAllocationExist(int id, CancellationToken arg2)
    {
        var leaveAllocation = await _allocationRepository.GetAsync(id);
        return leaveAllocation != null;
    }

    private async Task<bool> LeaveTypeMustExist(int id, CancellationToken arg3)
    {
        return await _typeRepository.ISLeaveTypeExist(id);
    }
}