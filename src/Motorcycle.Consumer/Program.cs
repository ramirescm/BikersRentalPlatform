using Microsoft.AspNetCore.Builder;
using Motorcycle.Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<MotorcycleEventConsumer>();
builder.Services.AddScoped<MotorcycleEventService>();

var app = builder.Build();
app.Run();