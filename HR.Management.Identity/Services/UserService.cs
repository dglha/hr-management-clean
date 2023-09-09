using System.Security.Claims;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.Management.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HR.Management.Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<List<Employee>> GetEmployees()
    {
        var leaveEmployees = await _userManager.GetUsersInRoleAsync("Employee");

        return leaveEmployees.Select(q => new Employee
        {
            Id = q.Id,
            Email = q.Email,
            FirstName = q.FirstName,
            LastName = q.LastName
        }).ToList();
    }

    public async Task<Employee> GetEmployee(string userId)
    {
        var leaveEmployee = await _userManager.FindByIdAsync(userId);
        return new Employee
        {
            Id = leaveEmployee.Id,
            Email = leaveEmployee.Email,
            FirstName = leaveEmployee.FirstName,
            LastName = leaveEmployee.LastName
        };
    }

    public string UserId
    {
        get => _contextAccessor.HttpContext?.User?.FindFirstValue("uid");
    }
}