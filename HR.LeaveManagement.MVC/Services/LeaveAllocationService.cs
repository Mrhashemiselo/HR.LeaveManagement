using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services;

public class LeaveAllocationService : BaseHttpService<ILeaveAllocationsClient>,
    ILeaveAllocationService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ILeaveAllocationsClient _leaveAllocationsClient;
    public LeaveAllocationService(ILeaveAllocationsClient leaveAllocationsClient,
        ILocalStorageService localStorageService) : base(localStorageService, leaveAllocationsClient)
    {
        _leaveAllocationsClient = leaveAllocationsClient;
        _localStorageService = localStorageService;
    }
    public async Task<Response<int>> CreateLeaveAllocation(int leaveTypeId)
    {
        try
        {
            var response = new Response<int>();
            CreateLeaveAllocationDto createLeaveAllocation = new()
            {
                LeaveTypeId = leaveTypeId
            };
            AddBearerToken(_leaveAllocationsClient.HttpClient);
            //var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocation);
            var apiResponse = await _client.PostAsync(createLeaveAllocation);
            if (apiResponse.Success)
            {
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
}
