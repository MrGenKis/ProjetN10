using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var ocelotConfigurationFile =
    builder.Environment.IsEnvironment("Docker")
        ? "ocelot.Docker.json"
        : "ocelot.json";

builder.Configuration.AddJsonFile(
    ocelotConfigurationFile,
    optional: false,
    reloadOnChange: true
);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();