
namespace HR.LeaveManagement.MVC.Services.Base;

public partial class AccountClient : IAccountClient
{
    public HttpClient HttpClient
    {
        get => _httpClient;
        set { }
    }
}
public partial class LeaveAllocationsClient : ILeaveAllocationsClient
{
    public HttpClient HttpClient
    {
        get => _httpClient;
        set { }
    }
}
public partial class LeaveRequestsClient : ILeaveRequestsClient
{
    public HttpClient HttpClient
    {
        get => _httpClient;
        set { }
    }
}
public partial class LeaveTypesClient : ILeaveTypesClient
{
    public HttpClient HttpClient
    {
        get => _httpClient;
        set { }
    }
}
//namespace HR.LeaveManagement.MVC.Services.Base;

//public partial class Client : IClient
//{
//    public HttpClient HttpClient
//    {
//        get => new HttpClient();
//        set { }
//    }
//}