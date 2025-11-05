using HR.LeaveManagement.Application;
using HR.LeaveManagement.Identity;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

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
app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

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

sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: Bearer 12345abcdef'",
                    Name = "Authorization"
                }
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }] = Array.Empty<string>()
                });
            }
        }
    }
}