using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.GridMap;
using Amazonia.DeliveryRoute.GridMap.Models;
using Amazonia.DeliveryRoute.RouteCalculation;
using Amazonia.DeliveryRoute.RouteCalculation.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<GridMapOptions>(builder.Configuration.GetSection(GridMapOptions.Section));

builder.Services.AddHttpClient<IGridService<Position>, GridService>((services, client) =>
{
    var options = services.GetRequiredService<IOptions<GridMapOptions>>();
    client.BaseAddress = new Uri(options.Value.GridSourceBaseUri);
});

builder.Services.AddTransient<IRouteCalculator<Position>, RouteCalculator>();

var app = builder.Build();

app.MapGet("/api/test", async context =>
{
    var start = new Position { X = "A", Y = 1 };
    var destination = new Position { X = "G", Y = 4 };

    await CalculateRoute(context, start, destination);
});

app.MapPost("/api", async context =>
{
    var positions = await context.Request.ReadFromJsonAsync<DistanceRequest>(cancellationToken: context.RequestAborted);

    if (positions is null)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    await CalculateRoute(context, positions.StartPoint, positions.DestinationPoint);
});

app.Run();

static async Task CalculateRoute(HttpContext context, Position start, Position destination)
{
    using var scope = context.RequestServices.CreateScope();

    var gridService = scope.ServiceProvider.GetRequiredService<IGridService<Position>>();
    var routeService = scope.ServiceProvider.GetRequiredService<IRouteCalculator<Position>>();

    var grid = await gridService.BuildGridAsync(context.RequestAborted);

    var route = await routeService.CalculateAsync(
        grid,
        start,
        destination,
        context.RequestAborted);

    await context.Response.WriteAsJsonAsync(route, PathContext.Default.RoutingResultPosition, cancellationToken: context.RequestAborted);
}

/// <summary>
/// Allows serialization of the Position Type on trimmed environments
/// </summary>
[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(RoutingResult<Position>))]
public partial class PathContext : JsonSerializerContext
{ }
