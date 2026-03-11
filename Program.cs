using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var slot = Environment.GetEnvironmentVariable("WEBSITE_SLOT_NAME") ?? "Unknown";

app.MapGet("/", () => $"Hello from .NET 8 Web App! Slot: {slot}");

app.Run();
