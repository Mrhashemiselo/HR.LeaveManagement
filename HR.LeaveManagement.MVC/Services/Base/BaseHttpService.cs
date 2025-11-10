using HR.LeaveManagement.MVC.Contracts;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.MVC.Services.Base;

public class BaseHttpService<TClient> where TClient : class
{
    protected readonly ILocalStorageService _localStorage;
    protected TClient _client;


    public BaseHttpService(ILocalStorageService localStorage, TClient client)
    {
        _localStorage = localStorage;
        _client = client;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
    {
        if (ex.StatusCode == 400)
        {
            return new Response<Guid>()
            {
                Message = "Validation errors have occurred",
                ValidationErrors = ex.Response,
                Success = false
            };
        }
        else if (ex.StatusCode == 404)
        {
            return new Response<Guid>()
            {
                Message = "The requested item could not be found.",
                Success = false
            };
        }
        else
        {
            return new Response<Guid>()
            {
                Message = "Something went wrong, please try again.",
                Success = false
            };
        }
    }

    protected void AddBearerToken(HttpClient httpClient)
    {
        if (_localStorage.Exists("token"))
        {
            var token = _localStorage.GetStorageValue<string>("token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
