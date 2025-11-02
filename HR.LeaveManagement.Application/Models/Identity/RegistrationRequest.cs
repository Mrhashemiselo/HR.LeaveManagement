using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Application.Models.Identity;
public class RegistrationRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(3)]
    public string Username { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
