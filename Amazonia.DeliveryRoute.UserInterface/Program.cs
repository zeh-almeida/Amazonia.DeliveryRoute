using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.UserInterface.Components;
using Amazonia.DeliveryRoute.UserInterface.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<DeliveryRouteOptions>(builder.Configuration.GetSection(DeliveryRouteOptions.Section));

builder.Services.AddHttpClient("DeliveryRoute.Api", (services, client) =>
{
    var options = services.GetRequiredService<IOptions<DeliveryRouteOptions>>();
    client.BaseAddress = new Uri(options.Value.ApiUrl);
});

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

/// <summary>
/// Allows serialization of the Response Model on trimmed environments
/// </summary>
[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(RoutingResult<Position>))]
public partial class ResultContext : JsonSerializerContext
{ }

/// <summary>
/// Allows serialization of the Request Model on trimmed environments
/// </summary>
[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(RoutingRequest<Position>))]
public partial class RequestContext : JsonSerializerContext
{ }
