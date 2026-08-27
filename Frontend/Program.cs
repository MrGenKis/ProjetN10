using Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var gatewayBaseUrl = builder.Configuration["Gateway:BaseUrl"]
    ?? throw new InvalidOperationException(
        "L'adresse de la Gateway n'est pas configurée."
    );

builder.Services.AddHttpClient<PatientApiService>(client =>
{
    client.BaseAddress = new Uri(gatewayBaseUrl);
});

builder.Services.AddHttpClient<NoteApiService>(client =>
{
    client.BaseAddress = new Uri(gatewayBaseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment()
    && !app.Environment.IsEnvironment("Docker"))
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patients}/{action=Index}/{id?}");

app.Run();