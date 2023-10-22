using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.RouteCalculation.Models;
using CommunityToolkit.Diagnostics;

namespace Amazonia.DeliveryRoute.RouteCalculation;

/// <summary>
/// Implements IRouteCalculator as a service
/// </summary>
public sealed record RouteCalculator<TValue>
    : IRouteCalculator<TValue>
    where TValue : class
{
    #region Properties
    private List<Vertice<GridItem<TValue>>> WorkingSet { get; set; }

    private Grid<TValue>? CurrentGrid { get; set; }

    private GridItem<TValue>? Start { get; set; }

    private GridItem<TValue>? Destination { get; set; }

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
    /// <exception cref="ArgumentException">
    /// Thrown if:
    ///  - grid is empty
    ///  - neither start nor destination are found in the grid
    ///  - start and destination are equal to eachother
    /// </exception>
    public async Task<IOrderedEnumerable<GridItem<TValue>>> CalculateAsync(
        Grid<TValue> grid,
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

        this.CurrentGrid = grid;

        this.Start = this.CurrentGrid.FindItem(start);
        if (this.Start is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(start));
        }

        this.Destination = this.CurrentGrid.FindItem(destination);
        if (this.Destination is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(destination));
        }

        if (start.Equals(destination))
        {
            throw new ArgumentException("Start position must be different", nameof(destination));
        }

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
            this.WorkingSet = new List<Vertice<GridItem<TValue>>>(itemCount)
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

    private Vertice<GridItem<TValue>> AddToWorkingSet(GridItem<TValue> gridItem)
    {
        var vertice = new Vertice<GridItem<TValue>>
        {
            Value = gridItem,
        };

        return this.AddToWorkingSet(vertice);
    }

    private Vertice<GridItem<TValue>> AddToWorkingSet(GridDistance<TValue> gridDistance)
    {
        var vertice = new Vertice<GridItem<TValue>>
        {
            Value = gridDistance.Other,
            Weight = gridDistance.Value,
        };

        return this.AddToWorkingSet(vertice);
    }

    private Vertice<GridItem<TValue>> AddToWorkingSet(Vertice<GridItem<TValue>> vertice)
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

    private Task<Vertice<GridItem<TValue>>> CalculateDistances()
    {
        return Task.Run(() =>
        {
            Vertice<GridItem<TValue>>? result = null;

            while (this.WorkingSet.Count > 0)
            {
                var item = this.WorkingSet.OrderBy(i => i.Distance).First();

                if (item.Value.Equals(this.Destination))
                {
                    result = item;
                    break;
                }

                foreach (var neighbor in item.Neighbors)
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

    private IOrderedEnumerable<GridItem<TValue>> BuildPathToDestination(Vertice<GridItem<TValue>> vertex)
    {
        this.CancellationToken.ThrowIfCancellationRequested();
        var path = new List<Vertice<GridItem<TValue>>>(this.CurrentGrid!.Count());

        if (vertex is not null)
        {
            while (vertex is not null)
            {
                path.Add(vertex);
                vertex = vertex!.Previous!;
            }
        }

        path.Reverse();
        return path.Select(i => i.Value).OrderBy(_ => 0);
    }
}
