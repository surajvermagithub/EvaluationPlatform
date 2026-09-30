//using CheckMate.API.Middleware;
//using CheckMate.Application.Interfaces.Repositories;
//using CheckMate.Application.Interfaces.Services;
//using CheckMate.Infrastructure.Data;
//using CheckMate.Infrastructure.Repositories;
//using CheckMate.Infrastructure.Services;
//using System.Text;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;

//using Microsoft.EntityFrameworkCore;
//using Serilog;
//using CheckMate.Application.Services;

//var projectDirectory = Directory.GetParent(
//    AppContext.BaseDirectory)!
//    .Parent!
//    .Parent!
//    .Parent!
//    .FullName;

//var logPath = Path.Combine(projectDirectory, "Logs");

//Directory.CreateDirectory(logPath);

//var logFileName = $"log-{DateTime.Now:yyyyMMdd-HHmmss}.txt";

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.File(
//        Path.Combine(logPath, logFileName))
//    .CreateLogger();

//var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp", policy =>
//    {
//        policy.WithOrigins(
//                "http://localhost:3000",                          // local dev ke liye rakho
//                "https://myapp-frontend.azurestaticapps.net"       // production
//              )
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});
//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<ITeacherService, TeacherService>();
//builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

//builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
//builder.Services.AddScoped<IInstituteRepository, InstituteRepository>();
//builder.Services.AddScoped<IInstituteService, InstituteService>();

//var jwtSettings = builder.Configuration.GetSection("Jwt");

//var jwtKey = jwtSettings["Key"]
//    ?? throw new InvalidOperationException("JWT key is not configured.");

//var jwtIssuer = jwtSettings["Issuer"]
//    ?? throw new InvalidOperationException("JWT issuer is not configured.");

//var jwtAudience = jwtSettings["Audience"]
//    ?? throw new InvalidOperationException("JWT audience is not configured.");

//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,

//            ValidIssuer = jwtIssuer,
//            ValidAudience = jwtAudience,

//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(jwtKey))
//        };
//    });

//builder.Services.AddAuthorization();

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseCors("AllowReactApp");
//app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//try
//{
//    Log.Information("Starting EvaluationPlatform API");

//    app.Run();
//}
//catch (Exception ex)
//{
//    Log.Fatal(ex, "Application terminated unexpectedly");
//}
//finally
//{
//    Log.CloseAndFlush();
//}


using CheckMate.API.Middleware;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Application.Interfaces.Services;
using CheckMate.Infrastructure.Data;
using CheckMate.Infrastructure.Repositories;
using CheckMate.Infrastructure.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Microsoft.EntityFrameworkCore;
using Serilog;
using CheckMate.Application.Services;

// ---- Logging setup (Console always, File only in Development) ----
var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console();

if (isDevelopment)
{
    var projectDirectory = Directory.GetParent(
        AppContext.BaseDirectory)!
        .Parent!
        .Parent!
        .Parent!
        .FullName;

    var logPath = Path.Combine(projectDirectory, "Logs");
    Directory.CreateDirectory(logPath);

    var logFileName = $"log-{DateTime.Now:yyyyMMdd-HHmmss}.txt";

    loggerConfig.WriteTo.File(Path.Combine(logPath, logFileName));
}

Log.Logger = loggerConfig.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",                                  
                "https://red-desert-0e9aef500.4.azurestaticapps.net"      
              )
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- Database ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- DI Registrations ----
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IInstituteRepository, InstituteRepository>();
builder.Services.AddScoped<IInstituteService, InstituteService>();

// ---- JWT Authentication ----
var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT key is not configured.");

var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException("JWT issuer is not configured.");

var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException("JWT audience is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ---- HTTP request pipeline ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting EvaluationPlatform API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
