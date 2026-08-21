using Microsoft.EntityFrameworkCore;
using PatientService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Migration automatique de la base
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    var retryCount = 0;
    const int maxRetries = 10;

    while (retryCount < maxRetries)
    {
        try
        {
            context.Database.Migrate();

            // Ajout des données de test
            DbInitializer.Initialize(context);

            break;
        }
        catch
        {
            retryCount++;

            Console.WriteLine(
                $"SQL Server n'est pas encore prêt. Tentative {retryCount}/{maxRetries}..."
            );

            Thread.Sleep(3000);
        }
    }
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();