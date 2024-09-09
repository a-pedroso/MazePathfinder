using FluentValidation;
using MazePathfinder.Api.Endpoints.Mazes;
using MazePathfinder.Api.ProgramExtensions;
using MazePathfinder.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(options =>
{
    options.UseInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
    options.DatabaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddProblemDetails();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidationExtension();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

// API Endpoints
app.MapMazesEndpoints();

app.Run();


// tests support
public partial class Program { }