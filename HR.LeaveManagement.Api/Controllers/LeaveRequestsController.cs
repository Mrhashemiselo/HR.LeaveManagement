using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveRequestsController(IMediator mediator) : ControllerBase
{
    // Get: api/<LeaveRequestsController>
    [HttpGet]
    public async Task<ActionResult<List<LeaveRequestDto>>> Get()
    {
        var leaveRequests = await mediator.Send(new GetLeaveRequestListRequest());
        return Ok(leaveRequests);
    }

    // Get api/<LeaveRequestsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveRequestDto>> Get(int id)
    {
        var leaveRequest = await mediator.Send(new GetLeaveRequestDetailRequest() { Id = id });
        return Ok(leaveRequest);
    }

    //Post api/<LeaveRequestsController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateLeaveRequestDto leaveRequest)
    {
        var command = new CreateLeaveRequestCommand()
        {
            LeaveRequestDto = leaveRequest
        };
        var response = await mediator.Send(command);
        return Ok(response);
    }

    //PUT api/<LeaveRequestsController>
    [HttpPut]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestDto leaveRequest)
    {
        var command = new UpdateLeaveRequestCommand()
        {
            LeaveRequestDto = leaveRequest,
            Id = id
        };
        await mediator.Send(command);
        return NoContent();
    }

    //PUT api/<LeaveRequestsController>/changeApproval/5
    [HttpPut("changeapproval/{id}")]
    public async Task<ActionResult> ChangeApproval(int id, [FromBody] ChangeLeaveRequestApprovalDto changeLeaveRequestApproval)
    {
        var command = new UpdateLeaveRequestCommand()
        {
            ChangeLeaveRequestApprovalDto = changeLeaveRequestApproval,
            Id = id
        };
        await mediator.Send(command);
        return NoContent();
    }

    //DELETE api/<LeaveRequestsController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteLeaveRequestCommand()
        {
            Id = id
        };
        await mediator.Send(command);
        return NoContent();
    }
}