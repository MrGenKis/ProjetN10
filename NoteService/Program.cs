using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NoteService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var mongoConnectionString =
    builder.Configuration["MongoDb:ConnectionString"]
    ?? throw new InvalidOperationException(
        "La chaîne de connexion MongoDB est absente."
    );

var mongoDatabaseName =
    builder.Configuration["MongoDb:DatabaseName"]
    ?? throw new InvalidOperationException(
        "Le nom de la base MongoDB est absent."
    );

var mongoClient = new MongoClient(mongoConnectionString);

builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseMongoDB(
        mongoClient,
        mongoDatabaseName
    )
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<NoteDbContext>();

    NoteDbInitializer.Initialize(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();