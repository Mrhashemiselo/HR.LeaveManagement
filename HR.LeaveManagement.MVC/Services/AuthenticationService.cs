using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.LeaveManagement.MVC.Services;

public class AuthenticationService : BaseHttpService<IAccountClient>,
    HR.LeaveManagement.MVC.Contracts.IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private JwtSecurityTokenHandler _tokenHandler;
    private readonly IMapper _mapper;
    private readonly IAccountClient _accountClient;

    public AuthenticationService(IAccountClient accountClient,
        ILocalStorageService localStorage,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper) : base(localStorage, accountClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenHandler = new JwtSecurityTokenHandler();
        _mapper = mapper;
        _accountClient = accountClient;
    }
    public async Task<bool> Authenticate(string email, string password)
    {
        try
        {
            AuthRequest authRequest = new AuthRequest()
            {
                Email = email,
                Password = password
            };

            var authenticationResponse = await _client.LoginAsync(authRequest);

            if (authenticationResponse.Token != string.Empty)
            {
                var tokenContent = _tokenHandler.ReadJwtToken(authenticationResponse.Token);
                var claims = ParseClaims(tokenContent);
                var user = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                var login = _httpContextAccessor.HttpContext
                    .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                _localStorage.SetStorageValue("token", authenticationResponse.Token);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task Logout()
    {
        _localStorage.ClearStorage(new List<string> { "token" });
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<bool> Register(RegisterVM registration)
    {

        RegistrationRequest registrationRequest = _mapper.Map<RegistrationRequest>(registration);
        var response = await _client.RegisterAsync(registrationRequest);

        if (!string.IsNullOrEmpty(response.UserId))
        {
            await Authenticate(registration.Email, registration.Password);
            return true;
        }
        return false;
    }

    private IList<Claim> ParseClaims(JwtSecurityToken token)
    {
        var claims = token.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, token.Subject));
        return claims;
    }
}
