namespace HR.LeaveManagement.MVC.Services.Base;
public partial interface IAccountClient
{
    public HttpClient HttpClient { get; set; }
}

public partial interface ILeaveAllocationsClient
{
    public HttpClient HttpClient { get; set; }
}

public partial interface ILeaveRequestsClient
{
    public HttpClient HttpClient { get; set; }
}

public partial interface ILeaveTypesClient
{
    public HttpClient HttpClient { get; set; }
}


//namespace HR.LeaveManagement.MVC.Services.Base;

//public partial interface IClient
//{
//    public HttpClient HttpClient { get; set; }
//}