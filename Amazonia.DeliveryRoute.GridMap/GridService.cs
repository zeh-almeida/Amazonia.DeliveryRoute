﻿using Amazonia.DeliveryRoute.Commons.Models;
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
/// Implements <see cref="IGridService"/> in order to build the Grid
/// </summary>
public sealed partial record GridService
    : IDisposable,
    IGridService
{
    #region Properties
    private HttpClient HttpClient { get; }

    private GridMapOptions Options { get; }

    private ILogger<IGridService> Logger { get; }

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
    public Task<Grid> BuildGridAsync(CancellationToken cancellationToken = default)
    {
        this.CurrentToken = cancellationToken;
        this.CurrentToken.ThrowIfCancellationRequested();

        return this.AcquireData();
    }

    private async Task<Grid> AcquireData()
    {
        var data = await this.CallApi();
        return this.ParseJsonIntoGrid(data);
    }

    private async Task<JsonObject> CallApi()
    {
        var data = await this.HttpClient.GetFromJsonAsync(
            this.Options.GridSourceApiUri,
            JsonContext.Default.JsonObject,
            cancellationToken: this.CurrentToken);

        return data!;
    }

    private Grid ParseJsonIntoGrid(JsonObject data)
    {
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