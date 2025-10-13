using System.Threading;
using AutoMapper;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler:IRequestHandler<CreateLeaveRequestCommand, int>
    {
        private readonly ILeaveRequestRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        public CreateLeaveRequestCommandHandler(IMapper mapper,
            ILeaveRequestRepository leaveAllocationRepository)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task<int> Handle(CreateLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(command.LeaveRequestDto);
            leaveRequest = await _leaveAllocationRepository.Add(leaveRequest);
            return leaveRequest.Id;
        }
    }
}