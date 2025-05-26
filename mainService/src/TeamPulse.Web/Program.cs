using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using TeamPulse.Performances.Infrastructure;
using TeamPulse.Team.Presentation;
using TeamPulse.Teams.Application;
using TeamPulse.Teams.Infrastructure;
using TeamPulse.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.WithEnvironmentName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddSerilog();
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TeamPulse api",
        Description = "Application for management it teams",
    }));

builder.Services
    .AddTeamApplication()
    .AddTeamInfrastructure(builder.Configuration)
    .AddTeamPresentation()
    
    .AddPerformancesInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();

namespace TeamPulse.Web
{
    public partial class Program;
}
