using Microsoft.AspNetCore.Builder;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var slot = Environment.GetEnvironmentVariable("WEBSITE_SLOT_NAME");
var hostname = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");

slot ??= "production";

app.MapGet("/", () => $@"
Hello from .NET 8 Web App!

Slot: {slot}
Hostname: {hostname}
");

app.Run();
