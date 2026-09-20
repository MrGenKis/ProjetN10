using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "La clé JWT est absente."
    );

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "L'émetteur JWT est absent."
    );

var jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "L'audience JWT est absente."
    );

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<PatientApiService>(client =>
{
    client.BaseAddress = new Uri(patientServiceUrl);
});

builder.Services.AddHttpClient<NoteApiService>(client =>
{
    client.BaseAddress = new Uri(noteServiceUrl);
});

builder.Services.AddScoped<RiskAssessmentService>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    )
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();