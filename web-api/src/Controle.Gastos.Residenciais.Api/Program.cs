using Controle.Gastos.Residenciais.Api.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddIdentityConfig(builder.Configuration);
builder.Services.AddApiConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddLoggingConfig(builder);
builder.Services.ResolveDependencies();

builder.AddDatabaseConfiguration();

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseApiConfig(app.Environment);
app.UseSwaggerConfig(apiVersionDescriptionProvider, app.Environment);
app.UseLoggingConfiguration();
app.UseDbSeed();

app.Run();