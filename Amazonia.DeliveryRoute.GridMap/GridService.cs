using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.GridMap.Models;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Amazonia.DeliveryRoute.GridMap;

public sealed partial record GridService
    : IDisposable,
    IGridService
{
    #region Constants
    private string Target = "1/10404696-fd43-4481-a7ed-f9369073252f";
    #endregion

    #region Properties
    private HttpClient HttpClient { get; }

    private GridMapOptions Options { get; }

    private ILogger<IGridService> Logger { get; }

    private bool IsDisposed { get; set; }

    private CancellationToken CurrentToken { get; set; }
    #endregion

    #region Constructors
    public GridService(
        IOptions<GridMapOptions> options,
        ILogger<IGridService> logger,
        HttpClient httpClient)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(options);
        Guard.IsNotNull(httpClient);

        this.Logger = logger;
        this.Options = options.Value;
        this.HttpClient = httpClient;
    }
    #endregion

    #region IDisposable
    private void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            if (disposing)
            {
                this.HttpClient.Dispose();
            }

            this.IsDisposed = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion

    public async Task<Grid> BuildGridAsync(CancellationToken cancellationToken = default)
    {
        this.CurrentToken = cancellationToken;
        this.CurrentToken.ThrowIfCancellationRequested();

        return await this.AcquireData();
    }

    private async Task<Grid> AcquireData()
    {
        var data = await this.HttpClient.GetFromJsonAsync<JsonObject>(
            this.Options.GridSourceApiUri,
            cancellationToken: this.CurrentToken);

        var grid = new Grid();

        foreach (var item in data)
        {
            var gridItem = ParseDataItem(grid, item.Key);

            foreach (var neighbor in item.Value.AsObject())
            {
                var neighborItem = ParseDataItem(grid, item.Key);
                gridItem.AddNeighbor(neighborItem, neighbor.Value.GetValue<decimal>());
            }
        }

        return grid;
    }

    private static GridItem ParseDataItem(Grid grid, string value)
    {
        var gridItem = new GridItem
        {
            Position = ParsePosition(value),
        };

        _ = grid.AddItem(gridItem);
        return gridItem;
    }

    private static Position ParsePosition(string value)
    {
        var parts = PositionRegex().Match(value);

        return new Position
        {
            X = parts.Groups["Alpha"].Value,
            Y = Convert.ToInt32(parts.Groups["Numeric"].Value),
        };
    }

    [GeneratedRegex(
    "(?<Alpha>[A-Z]*)(?<Numeric>[0-9]*)",
    RegexOptions.CultureInvariant
    | RegexOptions.Compiled
    | RegexOptions.Singleline)]
    private static partial Regex PositionRegex();
}
