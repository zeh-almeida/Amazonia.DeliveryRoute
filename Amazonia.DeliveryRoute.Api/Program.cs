using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.GridMap;
using Amazonia.DeliveryRoute.GridMap.Models;
using Amazonia.DeliveryRoute.RouteCalculation;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<GridMapOptions>(builder.Configuration.GetSection(GridMapOptions.Section));

builder.Services.AddHttpClient<IGridService<string>, GridService<string>>((services, client) =>
{
    var options = services.GetRequiredService<IOptions<GridMapOptions>>();
    client.BaseAddress = new Uri(options.Value.GridSourceBaseUri);
});

builder.Services.AddTransient<IRouteCalculator<string>, RouteCalculator<string>>();

var app = builder.Build();

app.MapGet("/", async context =>
{
    using var scope = context.RequestServices.CreateScope();
    var gridService = scope.ServiceProvider.GetRequiredService<IGridService<string>>();
    var routeService = scope.ServiceProvider.GetRequiredService<IRouteCalculator<string>>();

    var grid = await gridService.BuildGridAsync(context.RequestAborted);

    var start = new Position { X = "A", Y = 1 };
    var destination = new Position { X = "G", Y = 4 };

    var route = await routeService.CalculateAsync(grid, start, destination, context.RequestAborted);

    await context.Response.WriteAsJsonAsync(route, GridItemContext.Default.IEnumerableGridItemString, cancellationToken: context.RequestAborted);
});

app.Run();

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(IEnumerable<GridItem<string>>))]
public partial class GridItemContext : JsonSerializerContext
{ }
