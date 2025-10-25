using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks;
public static class MockLeaveTypeRepository
{
    public static Mock<ILeaveTypeRepository> GetLeaveTypeRepository()
    {
        var leaveTypes = new List<LeaveType>()
        {
            new LeaveType()
            {
                Id = 1,
                DefaultDays = 15,
                Name = "Test Vacation"
            },
            new LeaveType()
            {
                Id = 2,
                DefaultDays = 51,
                Name = "Test Sick"
            },
            new LeaveType()
            {
                Id = 3,
                DefaultDays = 20,
                Name= "Test Holyday"
            }
        };

        var mockRepo = new Mock<ILeaveTypeRepository>();

        mockRepo.Setup(s => s.GetAll()).ReturnsAsync(leaveTypes);

        mockRepo.Setup(s => s.Add(It.IsAny<LeaveType>()))
            .ReturnsAsync((LeaveType leaveType) =>
            {
                leaveTypes.Add(leaveType);
                return leaveType;
            });
        return mockRepo;
    }
}
