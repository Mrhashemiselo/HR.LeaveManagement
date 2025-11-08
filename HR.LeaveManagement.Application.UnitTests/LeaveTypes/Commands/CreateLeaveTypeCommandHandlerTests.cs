using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.LeaveTypes.Commands;
public class CreateLeaveTypeCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly CreateLeaveTypeDto _leaveTypeDto;
    private readonly CreateLeaveTypeCommandHandler _handler;

    public CreateLeaveTypeCommandHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();

        var mapperConfig = new MapperConfiguration(m =>
        {
            m.AddProfile<MappingProfile>();
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new CreateLeaveTypeCommandHandler(_mapper, _mockUow.Object);

        _leaveTypeDto = new CreateLeaveTypeDto()
        {
            DefaultDays = 15,
            Name = "Test Dto"
        };
    }

    [Fact]
    public async Task Valid_LeaveType_Added()
    {
        var result = await _handler.Handle(new CreateLeaveTypeCommand()
        {
            LeaveTypeDto = _leaveTypeDto
        }, CancellationToken.None);

        var leaveTypes = await _mockUow.Object.LeaveTypeRepository.GetAll();

        result.ShouldBeOfType<BaseCommandResponse>();

        leaveTypes.Count.ShouldBe(4);
    }

    [Fact]
    public async Task Invalid_LeaveType_Added()
    {
        _leaveTypeDto.DefaultDays = -1;

        var result = await _handler.Handle(new CreateLeaveTypeCommand()
        {
            LeaveTypeDto = _leaveTypeDto
        },
        CancellationToken.None);

        var leaveTypes = await _mockUow.Object.LeaveTypeRepository.GetAll();

        leaveTypes.Count.ShouldBe(3);

        result.ShouldBeOfType<BaseCommandResponse>();
    }
}
