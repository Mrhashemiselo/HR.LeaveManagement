using AutoMapper;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts;

public class LeaveTypeService : BaseHttpService, ILeaveTypeService
{
    private readonly ILocalStorageService _localStorage;
    private readonly IClient _httpClient;
    private readonly IMapper _mapper;

    /// <ImportantNote>
    /// _client Used from BaseHttpService class
    /// </ImportantNote>

    public LeaveTypeService(ILocalStorageService localStorage,
        IClient httpClient,
        IMapper mapper) : base(localStorage, httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM leaveType)
    {
        try
        {
            var response = new Response<int>();
            CreateLeaveTypeDto createLeaveType = _mapper.Map<CreateLeaveTypeDto>(leaveType);
            var apiResponse = await _client.LeaveTypesPOSTAsync(createLeaveType);
            if (apiResponse.Success)
            {
                response.Success = true;
                response.Data = apiResponse.Id;
            }
            else
            {
                foreach (var error in apiResponse.Errors)
                {
                    response.ValidationErrors = error + Environment.NewLine;
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
            await _client.LeaveRequestsDELETEAsync(id);
            return new Response<int>()
            {
                Success = true,
            };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }

    public async Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
    {
        var leaveType = await _client.LeaveAllocationsGETAsync(id);
        return _mapper.Map<LeaveTypeVM>(leaveType);
    }

    public async Task<List<LeaveTypeVM>> GetLeaveTypes()
    {
        var leaveTypes = await _client.LeaveTypesAllAsync();
        return _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
    }

    public async Task<Response<int>> UpdateLeaveType(LeaveTypeVM leaveType)
    {
        try
        {
            LeaveTypeDto leaveTypeDto = _mapper.Map<LeaveTypeDto>(leaveType);
            await _client.LeaveTypesPUTAsync(leaveTypeDto);
            return new Response<int>()
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<int>(ex);
        }
    }
}
