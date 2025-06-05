using System.Reflection;
using Microsoft.OpenApi.Models;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Enable OpenAPI/Swagger generation (with detailed info and XML comments)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define a single OpenAPI document (v1) with metadata
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "New backend service that provides resources for the Moon robot simulator.",
        Contact = new OpenApiContact
        {
            Name = "Romil Bijarnia",
            Email = "romil.bijarnia@deakin.edu.au"
        },
        Version = "v1"
    });
    // Include XML comments (auto-generated from code) for better documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger middleware and UI (only in development)
    app.UseSwagger();
    app.UseSwaggerUI(options => options.InjectStylesheet("/styles/theme-flattop.css"));
}

// Enable serving static files for custom Swagger UI assets (CSS, images, etc.)
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();
app.Run();
