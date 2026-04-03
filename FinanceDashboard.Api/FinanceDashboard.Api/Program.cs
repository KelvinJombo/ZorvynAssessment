using FinanceDashboard.Application.Interfaces.IServices;
using FinanceDashboard.Application.Services;
using FinanceDashboard.Application.Validators;
using FinanceDashboard.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection; 

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.   
builder.Services.AddInfrastructure(configuration);

builder.Services.AddScoped<IFinancialRecordService, FinancialRecordService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateFinancialRecordValidator)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
