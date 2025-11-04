using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Identity.Services;
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<Employee>> GetEmployees()
    {
        var employees = await _userManager.GetUsersInRoleAsync("Employee");
        return employees.Select(s => new Employee()
        {
            Id = s.Id,
            Email = s.Email,
            Firstname = s.FirstName,
            Lastname = s.LastName
        }).ToList();
    }
}
