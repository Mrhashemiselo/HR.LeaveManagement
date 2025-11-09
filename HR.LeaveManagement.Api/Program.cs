using HR.LeaveManagement.Api.Middlewares;
using HR.LeaveManagement.Application;
using HR.LeaveManagement.Identity;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
AddSwaggerDoc(builder.Services);
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Checkout API",
            Version = "v1",
            Description = "API for processing checkouts from cart."
        };
        return Task.CompletedTask;
    });
});

#region AddLayersConfiguration

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(c =>
{
    c.AddPolicy("CorsPolicy",
        p => p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Swagger");
    });
}
else
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Swagger");
    });
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseCors("CorsPolicy");

app.MapDefaultControllerRoute();

app.Run();


void AddSwaggerDoc(IServiceCollection services)
{
    services.AddOpenApi("internal", options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });
}