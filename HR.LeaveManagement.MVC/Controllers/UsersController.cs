using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers;
public class UsersController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public UsersController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginMV login, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Login Attempt failed. Please try again.");
        }
        else
        {
            returnUrl ??= Url.Content("~/");
            var isLoggedIn = await _authenticationService.Authenticate(login.Email, login.Password);
            if (isLoggedIn)
                return LocalRedirect(returnUrl);
        }

        return View(login);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registration)
    {
        if (ModelState.IsValid)
        {
            var returnUrl = Url.Content("~/");
            var isCreated = await _authenticationService.Register(registration);
            if (isCreated)
                return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
        return View(registration);
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        await _authenticationService.Logout();
        return LocalRedirect(returnUrl);
    }
}
