using HR.LeaveManagement.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HR.LeaveManagement.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, Assembly.GetExecutingAssembly());

        services.AddMediatR(m =>
            m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}