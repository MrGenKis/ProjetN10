using RiskService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var patientServiceUrl =
    builder.Configuration["Services:PatientServiceUrl"]
    ?? throw new InvalidOperationException(
        "L'adresse de PatientService est absente."
    );

var noteServiceUrl =
    builder.Configuration["Services:NoteServiceUrl"]
    ?? throw new InvalidOperationException(
        "L'adresse de NoteService est absente."
    );

builder.Services.AddHttpClient<PatientApiService>(client =>
{
    client.BaseAddress = new Uri(patientServiceUrl);
});

builder.Services.AddHttpClient<NoteApiService>(client =>
{
    client.BaseAddress = new Uri(noteServiceUrl);
});

builder.Services.AddScoped<RiskAssessmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();