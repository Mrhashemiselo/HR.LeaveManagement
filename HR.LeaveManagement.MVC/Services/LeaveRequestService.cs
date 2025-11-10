using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models.LeaveAllocation;
using HR.LeaveManagement.MVC.Models.LeaveRequest;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services;

public class LeaveRequestService : BaseHttpService<ILeaveRequestsClient>,
    ILeaveRequestService
{
    private readonly IMapper _mapper;
    private readonly ILeaveRequestsClient _leaveRequestsClient;
    private readonly ILocalStorageService _localStorageService;
    public LeaveRequestService(IMapper mapper,
        ILeaveRequestsClient leaveRequestsClient,
        ILocalStorageService localStorageService) : base(localStorageService, leaveRequestsClient)
    {
        _mapper = mapper;
        _leaveRequestsClient = leaveRequestsClient;
        _localStorageService = localStorageService;
    }

    public async Task ApproveLeaveRequest(int id, bool approved)
    {
        AddBearerToken(_leaveRequestsClient.HttpClient);
        try
        {
            var request = new ChangeLeaveRequestApprovalDto { Approved = approved, Id = id };
            //await _client.ChangeapprovalAsync(id, request);
            await _client.ChangeApprovalAsync(id, request);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
    {
        try
        {
            var response = new Response<int>();
            CreateLeaveRequestDto createLeaveRequest = _mapper.Map<CreateLeaveRequestDto>(leaveRequest);
            AddBearerToken(_leaveRequestsClient.HttpClient);
            //var apiResponse = await _client.LeaveRequestsPOSTAsync(createLeaveRequest);
            var apiResponse = await _client.PostAsync(createLeaveRequest);
            if (apiResponse.Success)
            {
                response.Data = apiResponse.Id;
                response.Success = true;
            }
            else
            {
                foreach (var error in apiResponse.Errors)
                {
                    response.ValidationErrors += error + Environment.NewLine;
                }
            }
            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }

    public Task DeleteLeaveRequest(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
    {
        AddBearerToken(_leaveRequestsClient.HttpClient);
        //var leaveRequests = await _client.LeaveRequestsAllAsync(isLoggedInUser: false);
        var leaveRequests = await _client.GetAllAsync(isLoggedInUser: false);

        var model = new AdminLeaveRequestViewVM
        {
            TotalRequests = leaveRequests.Count,
            ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
            PendingRequests = leaveRequests.Count(q => q.Approved == null),
            RejectedRequests = leaveRequests.Count(q => q.Approved == false),
            LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
        };
        return model;
    }

    public async Task<LeaveRequestVM> GetLeaveRequest(int id)
    {
        AddBearerToken(_leaveRequestsClient.HttpClient);
        //var leaveRequest = await _client.LeaveRequestsGETAsync(id);
        var leaveRequest = await _client.GetAsync(id);
        return _mapper.Map<LeaveRequestVM>(leaveRequest);
    }

    public async Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests()
    {
        AddBearerToken(_leaveRequestsClient.HttpClient);
        //var leaveRequests = await _client.LeaveRequestsAllAsync(isLoggedInUser: true);
        var leaveRequests = await _client.GetAllAsync(isLoggedInUser: true);
        //var allocations = await _client.LeaveAllocationsAllAsync(isLoggedIn: true);
        var allocations = await _client.GetAllAsync(isLoggedInUser: true);
        var model = new EmployeeLeaveRequestViewVM
        {
            LeaveAllocations = _mapper.Map<List<LeaveAllocationVM>>(allocations),
            LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
        };

        return model;
    }
}
