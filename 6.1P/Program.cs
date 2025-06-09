using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using robot_controller_api.Persistence;
using robot_controller_api.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register your data access interfaces and implementations
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
// builder.Services.AddScoped<IMapDataAccess, MapADO>();

builder.Services.AddScoped<IUserDataAccess, UserRepository>();

builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
builder.Services.AddScoped<IMapDataAccess, MapRepository>();

builder.Services.AddCors();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
// ✅ Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});

var app = builder.Build();


app.UseRouting();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();