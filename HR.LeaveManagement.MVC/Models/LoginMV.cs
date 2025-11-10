using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.MVC.Models;

public class LoginMV
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string? ReturnUrl { get; set; }
}
