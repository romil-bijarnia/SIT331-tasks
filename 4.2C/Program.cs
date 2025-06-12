using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using robot_controller_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register your data access interfaces and implementations
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
// builder.Services.AddScoped<IMapDataAccess, MapADO>();


builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
builder.Services.AddScoped<IMapDataAccess, MapRepository>();

builder.Services.AddCors();

var app = builder.Build();


app.UseRouting();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
