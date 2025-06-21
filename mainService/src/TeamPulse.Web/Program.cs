using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using TeamPulse.Accounts.Application;
using TeamPulse.Accounts.Infrastructure;
using TeamPulse.Accounts.Infrastructure.Jwt;
using TeamPulse.Accounts.Infrastructure.Options;
using TeamPulse.Accounts.Infrastructure.Seeding;
using TeamPulse.Accounts.Presentation;
using TeamPulse.Framework.Authorization;
using TeamPulse.Performances.Application;
using TeamPulse.Performances.Infrastructure;
using TeamPulse.Performances.Presentation;
using TeamPulse.Reports.Application;
using TeamPulse.Team.Presentation;
using TeamPulse.Teams.Application;
using TeamPulse.Teams.Infrastructure;
using TeamPulse.Web.Middlewares;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

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
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TeamPulse api",
        Description = "Application for management it teams",
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header, 
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey 
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { 
            new OpenApiSecurityScheme 
            { 
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" 
                } 
            },
            new string[] { } 
        } 
    });
});


builder.Services
    .AddTeamApplication()
    .AddTeamInfrastructure(builder.Configuration)
    .AddTeamPresentation()
    
    .AddPerformancesApplication()
    .AddPerformancesInfrastructure(builder.Configuration)
    .AddPerformancesPresentation()

    .AddReportsApplication()
    
    .AddAccountsApplication()
    .AddAccountsInfrastructure(builder.Configuration)
    .AddAccountPresentation();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection(JwtOptions.SECTION_NAME).Get<JwtOptions>()
                         ?? throw new ApplicationException("Missing JWT configuration");

        options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
    });


builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

builder.Services.AddAuthorization();

var app = builder.Build();

var accountSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await accountSeeder.SeedAsync(CancellationToken.None);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseCors(options =>
{
    options
        .WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseSerilogRequestLogging();
app.MapControllers();


app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/users", () =>
{
    List<string> users = ["user1", "user2", "user3", "user4", "user5"];

    return Results.BadRequest();
});

app.Run();

namespace TeamPulse.Web
{
    public partial class Program;
}
