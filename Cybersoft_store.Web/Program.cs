var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//DI HttpClient
builder.Services.AddHttpClient("CybersoftMarketPlace.Web.ServerAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5018");
});

builder.Services.AddScoped<ProductStateService>();

var app = builder.Build();



app.UseRouting();

app.MapStaticAssets();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
