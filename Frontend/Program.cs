using Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddTransient<AuthTokenHandler>();

var gatewayBaseUrl = builder.Configuration["Gateway:BaseUrl"]
    ?? throw new InvalidOperationException(
        "L'adresse de la Gateway n'est pas configurée."
    );

builder.Services.AddHttpClient<PatientApiService>(client =>
{
    client.BaseAddress = new Uri(gatewayBaseUrl);
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<NoteApiService>(client =>
{
    client.BaseAddress = new Uri(gatewayBaseUrl);
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<RiskApiService>(client =>
{
    client.BaseAddress = new Uri(gatewayBaseUrl);
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<AuthApiService>(client =>
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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.MapGet("/", context =>
{
    context.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

app.Run();