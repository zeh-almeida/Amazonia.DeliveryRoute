using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.RouteCalculation.Models;
using CommunityToolkit.Diagnostics;

namespace Amazonia.DeliveryRoute.RouteCalculation;

/// <summary>
/// Implements IRouteCalculator as a service
/// </summary>
public sealed record RouteCalculator : IRouteCalculator
{
    #region Properties
    private List<Vertice<GridItem>> WorkingSet { get; set; }

    private Grid? CurrentGrid { get; set; }

    private GridItem? Start { get; set; }

    private GridItem? Destination { get; set; }

    private CancellationToken CancellationToken { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new RouteCalculator
    /// </summary>
    public RouteCalculator()
    {
        this.WorkingSet = [];
    }
    #endregion

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IOrderedEnumerable<GridItem>> CalculateAsync(
        Grid grid,
        Position start,
        Position destination,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(grid);
        Guard.IsNotNull(start);
        Guard.IsNotNull(destination);

        this.CancellationToken = cancellationToken;

        if (grid.IsEmpty())
        {
            throw new ArgumentException("Cannot be empty", nameof(grid));
        }

        if (grid.FindItem(start) is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(start));
        }

        if (grid.FindItem(destination) is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(destination));
        }

        this.CurrentGrid = grid;

        this.Start = this.CurrentGrid.FindItem(start)!;
        this.Destination = this.CurrentGrid.FindItem(destination)!;

        this.CancellationToken.ThrowIfCancellationRequested();

        await this.PrepareVertices();
        var finalVertex = await this.CalculateDistances();

        return this.BuildPathToDestination(finalVertex);
    }

    #region Preparation
    private Task PrepareVertices()
    {
        return Task.Run(() =>
        {
            var items = this.CurrentGrid!.AsEnumerable();
            var itemCount = this.CurrentGrid!.Count();

            this.WorkingSet.Clear();
            this.WorkingSet = new List<Vertice<GridItem>>(itemCount)
            {
                new() {
                    Value = this.Start!,
                    Weight = 0,
                    Distance = 0,
                },
            };

            foreach (var item in items)
            {
                var added = this.AddToWorkingSet(item);

                foreach (var neighbor in item.AllNeighbors())
                {
                    var neighborVertice = this.AddToWorkingSet(neighbor);
                    added.Neighbors.Add(neighborVertice);
                }
            }
        }, this.CancellationToken);
    }

    private Vertice<GridItem> AddToWorkingSet(GridItem gridItem)
    {
        var vertice = new Vertice<GridItem>
        {
            Value = gridItem,
        };

        return this.AddToWorkingSet(vertice);
    }

    private Vertice<GridItem> AddToWorkingSet(GridDistance gridDistance)
    {
        var vertice = new Vertice<GridItem>
        {
            Value = gridDistance.Other,
            Weight = gridDistance.Value,
        };

        return this.AddToWorkingSet(vertice);
    }

    private Vertice<GridItem> AddToWorkingSet(Vertice<GridItem> vertice)
    {
        var existing = this.WorkingSet.Find(item => item.Equals(vertice));

        if (existing is not null)
        {
            return existing;
        }

        this.WorkingSet.Add(vertice);
        return vertice;
    }
    #endregion

    private Task<Vertice<GridItem>> CalculateDistances()
    {
        return Task.Run(() =>
        {
            Vertice<GridItem>? result = null;

            while (this.WorkingSet.Count > 0)
            {
                var item = this.WorkingSet.OrderBy(i => i.Distance).First();

                if (item.Value.Equals(this.Destination))
                {
                    result = item;
                    break;
                }

                foreach (var neighbor in item.Neighbors.OrderBy(x => x.Distance))
                {
                    var calculatedDistance = item.Distance + neighbor.Weight;
                    if (calculatedDistance < neighbor.Distance)
                    {
                        neighbor.Distance = calculatedDistance;
                        neighbor.Previous = item;

                        _ = this.WorkingSet.Remove(neighbor);
                        this.WorkingSet.Add(neighbor);
                    }
                }

                _ = this.WorkingSet.Remove(item);
            }

            return result!;
        }, this.CancellationToken);
    }

    private IOrderedEnumerable<GridItem> BuildPathToDestination(Vertice<GridItem> vertex)
    {
        this.CancellationToken.ThrowIfCancellationRequested();
        var path = new List<Vertice<GridItem>>(this.CurrentGrid!.Count());

        if (vertex is not null)
        {
            while (vertex is not null)
            {
                path.Add(vertex);
                vertex = vertex.Previous;
            }
        }

        path.Reverse();
        return path.Select(i => i.Value).OrderBy(_ => 0);
    }
}
