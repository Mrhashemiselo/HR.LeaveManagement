using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
public class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<DeleteLeaveTypeCommand>
{
    public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var leaveType = await leaveTypeRepository.Get(request.Id);
        await leaveTypeRepository.Delete(leaveType);
        return Unit.Value;
    }
}
