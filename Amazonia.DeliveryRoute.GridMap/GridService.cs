using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.GridMap.Models;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Amazonia.DeliveryRoute.GridMap;

/// <summary>
/// Implements <see cref="IGridService{TValue}"/> in order to build the Grid
/// </summary>
public sealed partial record GridService<TValue>
    : IDisposable,
    IGridService<TValue>
    where TValue : class
{
    #region Properties
    private HttpClient HttpClient { get; }

    private GridMapOptions Options { get; }

    private ILogger<IGridService<TValue>> Logger { get; }

    private bool IsDisposed { get; set; }

    private CancellationToken CurrentToken { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new GridService
    /// </summary>
    /// <param name="options">Options for API endpoint address</param>
    /// <param name="logger">Log for service operations</param>
    /// <param name="httpClient">Client to make API calls</param>
    public GridService(
        IOptions<GridMapOptions> options,
        ILogger<IGridService<TValue>> logger,
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
    [ExcludeFromCodeCoverage]
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

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion

    /// <inheritdoc/>
    /// <remarks>calls an external HTTP API in order to build the Grid</remarks>
    public Task<Grid<TValue>> BuildGridAsync(CancellationToken cancellationToken = default)
    {
        this.CurrentToken = cancellationToken;
        this.CurrentToken.ThrowIfCancellationRequested();

        return this.AcquireData();
    }

    private async Task<Grid<TValue>> AcquireData()
    {
        var data = await this.CallApi();
        return this.ParseJsonIntoGrid(data);
    }

    private async Task<JsonObject> CallApi()
    {
        JsonObject? result = null;

        try
        {
            result = await this.HttpClient.GetFromJsonAsync(
            this.Options.GridSourceApiUri,
            JsonContext.Default.JsonObject,
            cancellationToken: this.CurrentToken);
        }
        catch (HttpRequestException ex)
        {
            // Create an empty JSON object in order to have an empty grid.
            // Error gets logged in order to be searchable later.
            this.Logger.LogError(ex, "Error calling the Grid API");
            result = [];
        }

        return result!;
    }

    private Grid<TValue> ParseJsonIntoGrid(JsonObject data)
    {
        var grid = new Grid<TValue>();

        if (0.Equals(data.Count))
        {
            return grid;
        }

        foreach (var item in data)
        {
            var gridItem = ParseDataItem(item.Key);

            foreach (var neighbor in item.Value!.AsObject())
            {
                var neighborItem = ParseDataItem(neighbor.Key);
                gridItem.AddNeighbor(neighborItem, neighbor.Value!.GetValue<decimal>());
            }

            _ = grid.AddItem(gridItem);
        }

        return grid;
    }

    private static GridItem<TValue> ParseDataItem(string value)
    {
        var gridItem = new GridItem<TValue>
        {
            Position = ParsePosition(value),
        };

        return gridItem;
    }

    /// <summary>
    /// Parses a string into a valid <see cref="Position"/>
    /// </summary>
    /// <param name="value">string to convert from</param>
    /// <returns>parsed position</returns>
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

/// <summary>
/// Implements <see cref="JsonSerializerContext"/> for <see cref="JsonObject"/> to be used in AOT context
/// </summary>
[JsonSerializable(typeof(JsonObject), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class JsonContext : JsonSerializerContext
{ }
