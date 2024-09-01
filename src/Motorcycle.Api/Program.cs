using Motorcycle.Api.Extensions;
using Motorcycle.Shared.Config;

var builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = new();
builder.Configuration.Bind(appSettings);
builder.Services.AddSingleton(appSettings);

builder.Services.ConfigureAppDependencies(builder.Configuration, appSettings);

var app = builder.Build();

app.UseApiConfiguration();

app.Run();