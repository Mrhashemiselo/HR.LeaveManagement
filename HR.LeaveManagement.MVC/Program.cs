using HR.LeaveManagement.MVC;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Middlewares;
using HR.LeaveManagement.MVC.Services;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/users/login");
                });


//builder.Services.AddHttpClient<IClient, Client>(c => c.BaseAddress = new Uri("https://localhost:7284"));
builder.Services.AddHttpClient<IAccountClient, AccountClient>(c => c.BaseAddress = new Uri("https://localhost:7087"));
builder.Services.AddHttpClient<ILeaveAllocationsClient, LeaveAllocationsClient>(c => c.BaseAddress = new Uri("https://localhost:7087"));
builder.Services.AddHttpClient<ILeaveRequestsClient, LeaveRequestsClient>(c => c.BaseAddress = new Uri("https://localhost:7087"));
builder.Services.AddHttpClient<ILeaveTypesClient, LeaveTypesClient>(c => c.BaseAddress = new Uri("https://localhost:7087"));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, Assembly.GetExecutingAssembly());

#region IOC
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
#endregion
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCookiePolicy();
app.UseAuthentication();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<RequestMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
