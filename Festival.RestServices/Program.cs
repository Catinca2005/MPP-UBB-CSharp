using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register controllers so the framework can discover and map ShowController
builder.Services.AddControllers();

// Enable SignalR engine for real-time WebSockets communication
builder.Services.AddSignalR();

// Register OpenAPI (Swagger) to automatically generate API documentation
builder.Services.AddOpenApi();

// Allow React frontend to communicate with this API
// Allow React frontend to communicate with this API (Required for SignalR)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Exact React URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
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

// Expose the SignalR Hub on a specific URL path
app.MapHub<Festival.RestServices.Hubs.ShowHub>("/showHub");

// Force the application to listen on a fixed port to avoid launchSettings conflicts
app.Run("http://localhost:5050");