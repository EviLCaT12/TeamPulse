using AuthService.Database;
using AuthService.Domain;
using AuthService.EmailSender;
using AuthService.Features;
using AuthService.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<AuthDbContext>(_ => 
    new AuthDbContext(builder.Configuration.GetConnectionString("Database")!));

builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<VerifyEmailHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IMailSender, EmailSender>();


builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    

builder.Services
    .AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
}); 

builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(MailOptions.SECTION_NAME));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SECTION_NAME));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "AuthService"));
}

app.MapControllers();

app.Run();

