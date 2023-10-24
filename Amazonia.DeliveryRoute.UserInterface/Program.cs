using Amazonia.DeliveryRoute.UserInterface.Components;
using Amazonia.DeliveryRoute.UserInterface.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<DeliveryRouteOptions>(builder.Configuration.GetSection(DeliveryRouteOptions.Section));

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
    _ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
