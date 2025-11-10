using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models.LeaveType;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services;

public class LeaveTypeService : BaseHttpService<ILeaveTypesClient>,
    ILeaveTypeService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IMapper _mapper;
    private readonly ILeaveTypesClient _leaveTypesClient;

    public LeaveTypeService(IMapper mapper,
        ILocalStorageService localStorageService,
        ILeaveTypesClient leaveTypesClient) : base(localStorageService, leaveTypesClient)
    {
        _localStorageService = localStorageService;
        _mapper = mapper;
        _leaveTypesClient = leaveTypesClient;
    }

    public async Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM leaveType)
    {
        try
        {
            var response = new Response<int>();
            CreateLeaveTypeDto createLeaveType = _mapper.Map<CreateLeaveTypeDto>(leaveType);
            AddBearerToken(_leaveTypesClient.HttpClient);
            //var apiResponse = await _client.LeaveTypesPOSTAsync(createLeaveType);
            var apiResponse = await _client.PostAsync(createLeaveType);
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

    public async Task<Response<int>> DeleteLeaveType(int id)
    {
        try
        {
            AddBearerToken(_leaveTypesClient.HttpClient);
            //await _client.LeaveTypesDELETEAsync(id);
            await _client.DeleteAsync(id);
            return new Response<int>() { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }

    public async Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
    {
        AddBearerToken(_leaveTypesClient.HttpClient);
        //var leaveType = await _client.LeaveTypesGETAsync(id);
        var leaveType = await _client.GetAsync(id);
        return _mapper.Map<LeaveTypeVM>(leaveType);
    }

    public async Task<List<LeaveTypeVM>> GetLeaveTypes()
    {
        AddBearerToken(_leaveTypesClient.HttpClient);
        //var leaveTypes = await _client.LeaveTypesAllAsync();
        var leaveTypes = await _client.GetAllAsync();
        return _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
    }

    public async Task<Response<int>> UpdateLeaveType(int id, LeaveTypeVM leaveType)
    {
        try
        {
            LeaveTypeDto leaveTypeDto = _mapper.Map<LeaveTypeDto>(leaveType);
            AddBearerToken(_leaveTypesClient.HttpClient);
            //await _client.LeaveTypesPUTAsync(id.ToString(), leaveTypeDto);
            await _client.PutAsync(id.ToString(), leaveTypeDto);
            return new Response<int>() { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }

}