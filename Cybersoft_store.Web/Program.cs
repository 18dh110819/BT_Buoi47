var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalStorageServices();

// DI HttpClient (name aligned with services)
builder.Services.AddHttpClient("CybersoftShopee", client =>
{
    client.BaseAddress = new Uri("http://localhost:5098");
});

builder.Services.AddScoped<ProductStateService>();
builder.Services.AddScoped<UserStateService>();

var app = builder.Build();



app.UseRouting();

app.MapStaticAssets();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
