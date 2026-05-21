using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register controllers so the framework can discover and map ShowController
builder.Services.AddControllers();

// Register OpenAPI (Swagger) to automatically generate API documentation
builder.Services.AddOpenApi();

// Allow React frontend to communicate with this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Apply the CORS policy
app.UseCors("AllowReact");

// Enable development tools like OpenAPI documentation
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Map the incoming HTTP requests to the appropriate Controller endpoints
app.MapControllers();

// Force the application to listen on a fixed port to avoid launchSettings conflicts
app.Run("http://localhost:5050");