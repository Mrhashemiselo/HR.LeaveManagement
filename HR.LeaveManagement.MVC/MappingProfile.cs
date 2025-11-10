using AutoMapper;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Models.LeaveAllocation;
using HR.LeaveManagement.MVC.Models.LeaveRequest;
using HR.LeaveManagement.MVC.Models.LeaveType;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateLeaveTypeDto, CreateLeaveTypeVM>().ReverseMap();

        CreateMap<HR.LeaveManagement.MVC.Services.Base.CreateLeaveRequestDto, CreateLeaveRequestVM>().ReverseMap();

        CreateMap<HR.LeaveManagement.MVC.Services.Base.LeaveRequestDto, LeaveRequestVM>()
            .ForMember(q => q.DateRequested,
            opt => opt.MapFrom(x => x.DateRequested.DateTime))
            .ForMember(q => q.StartDate,
            opt => opt.MapFrom(x => x.StartDate.DateTime))
            .ForMember(q => q.EndDate,
            opt => opt.MapFrom(x => x.EndDate.DateTime))
            .ReverseMap();

        CreateMap<HR.LeaveManagement.MVC.Services.Base.LeaveRequestListDto, LeaveRequestVM>()
            .ForMember(q => q.DateRequested,
            opt => opt.MapFrom(x => x.DateRequested.DateTime))
            .ForMember(q => q.StartDate,
            opt => opt.MapFrom(x => x.StartDate.DateTime))
            .ForMember(q => q.EndDate,
            opt => opt.MapFrom(x => x.EndDate.DateTime))
            .ReverseMap();

        CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();

        CreateMap<LeaveAllocationDto, LeaveAllocationVM>().ReverseMap();

        CreateMap<RegisterVM, HR.LeaveManagement.MVC.Services.Base.RegistrationRequest>()
            .ForMember(dest => dest.Username,
            opt => opt.MapFrom(src => src.UserName))
            .ReverseMap();

        CreateMap<EmployeeVM, HR.LeaveManagement.MVC.Services.Base.Employee>().ReverseMap();
    }
}
