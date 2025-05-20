using BeingSearchApp.Server.Data;
using BeingSearchApp.Server.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Configure CORS from appsettings.json
var corsConfig = builder.Configuration.GetSection("Cors");
var allowedOrigins = corsConfig.GetSection("AllowedOrigins").Get<string[]>();
var allowedMethods = corsConfig.GetSection("AllowedMethods").Get<string[]>();
var allowedHeaders = corsConfig.GetSection("AllowedHeaders").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        // Configure origins
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin();
        }
        else
        {
            policy.WithOrigins(allowedOrigins);
        }

        // Configure methods
        if (allowedMethods.Contains("*"))
        {
            policy.AllowAnyMethod();
        }
        else
        {
            policy.WithMethods(allowedMethods);
        }

        // Configure headers
        if (allowedHeaders.Contains("*"))
        {
            policy.AllowAnyHeader();
        }
        else
        {
            policy.WithHeaders(allowedHeaders);
        }

        
    });
});

// Register repository and service
builder.Services.AddSingleton<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddSingleton<ICsvImportService, CsvImportService>();

// Add Swagger/OpenAPI support
var swaggerConfig = builder.Configuration.GetSection("Swagger");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        swaggerConfig["Version"] ?? "v1",
        new OpenApiInfo
        {
            Title = swaggerConfig["Title"] ?? "Locations API",
            Version = swaggerConfig["Version"] ?? "v1",
            Description = swaggerConfig["Description"] ?? "API for retrieving locations with availability between 10 AM and 1 PM",
            Contact = new OpenApiContact
            {
                Name = "Developer",
                Email = "developer@example.com"
            }
        });

    // Include XML comments if available
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Remove this line as we're adding Swagger explicitly
// builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/{swaggerConfig["Version"] ?? "v1"}/swagger.json", swaggerConfig["Title"] ?? "Locations API");
        c.RoutePrefix = "swagger"; // Access Swagger UI at /swagger
    });

    
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();