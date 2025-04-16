using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Clear and reconfigure configuration sources if needed
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configure services using our extension method
builder.Services.AddCustomConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline using our extension method
app.UseCustomPipeline(app.Environment);

app.Run();