using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.GridMap;
using Amazonia.DeliveryRoute.GridMap.Models;
using Amazonia.DeliveryRoute.RouteCalculation;
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

app.MapGet("/", async context =>
{
    using var scope = context.RequestServices.CreateScope();
    var gridService = scope.ServiceProvider.GetRequiredService<IGridService<Position>>();
    var routeService = scope.ServiceProvider.GetRequiredService<IRouteCalculator<Position>>();

    var grid = await gridService.BuildGridAsync(context.RequestAborted);

    var start = grid.FindItem(new Position { X = "A", Y = 1 })!;
    var destination = grid.FindItem(new Position { X = "G", Y = 4 })!;

    var route = await routeService.CalculateAsync(start, destination, context.RequestAborted);

    await context.Response.WriteAsJsonAsync(route, VertexContext.Default.IEnumerableVertexPosition, cancellationToken: context.RequestAborted);
});

app.Run();

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(IEnumerable<Vertex<Position>>))]
public partial class VertexContext : JsonSerializerContext
{ }
