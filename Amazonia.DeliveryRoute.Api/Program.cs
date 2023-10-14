using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.GridMap;
using Amazonia.DeliveryRoute.GridMap.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<GridMapOptions>(builder.Configuration.GetSection(GridMapOptions.Section));

builder.Services.AddHttpClient<IGridService, GridService>((services, client) =>
{
    var options = services.GetRequiredService<IOptions<GridMapOptions>>();
    client.BaseAddress = new Uri(options.Value.GridSourceBaseUri);
});

var app = builder.Build();

app.MapGet("/", async context =>
{
    using var scope = context.RequestServices.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<IGridService>();

    var grid = await service.BuildGridAsync(context.RequestAborted);
    await context.Response.WriteAsJsonAsync(grid.AsEnumerable(), GridItemContext.Default.IEnumerableGridItem, cancellationToken: context.RequestAborted);
});

app.Run();

[ExcludeFromCodeCoverage]
[JsonSerializable(typeof(IEnumerable<GridItem>))]
public partial class GridItemContext : JsonSerializerContext
{ }
