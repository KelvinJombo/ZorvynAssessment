using AutoMapper;
using FinanceDashboard.Api;
using FinanceDashboard.Api.AutoMapper;
using FinanceDashboard.Api.Middleware;
using FinanceDashboard.Application.Interfaces;
using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Services;
using FinanceDashboard.Application.Validators;
using FinanceDashboard.Commons.Utilities;
using FinanceDashboard.Domain.Models;
using FinanceDashboard.Infrastructure.Context;
using FinanceDashboard.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using StatusCodes = FinanceDashboard.Commons.Utilities.StatusCodes;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.   
builder.Services.AddInfrastructure(configuration);
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DashboardDbContext>()
    .AddDefaultTokenProviders();


var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddRateLimiter(options =>
{
    //Global limiter (fallback)
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var userId = context.User?.Identity?.IsAuthenticated == true
            ? context.User.FindFirst("sub")?.Value ?? "authenticated-user"
            : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: userId,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5, // max requests
                Window = TimeSpan.FromSeconds(30), // per 30 seconds
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var response = new Response<string>
        {
            StatusCode = StatusCodes.TooManyRequests,
            Message = ResponseMessages.TooManyRequests,
            Data = null
        };

        await context.HttpContext.Response.WriteAsJsonAsync(response, token);
    };


    //Named policy (for specific endpoints)
    options.AddPolicy("Strict", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(30),
                QueueLimit = 0
            }));

    options.RejectionStatusCode = StatusCodes.TooManyRequests;
});


builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(MapperProfiles));
builder.Services.AddScoped<IFinancialRecordService, FinancialRecordService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateFinancialRecordValidator)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed database with roles and users
using (var scope = app.Services.CreateScope())
{
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
