using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add minimal services (controllers as services)
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
