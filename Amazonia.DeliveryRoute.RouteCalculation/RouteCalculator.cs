using Amazonia.DeliveryRoute.Commons.Models;
using CommunityToolkit.Diagnostics;

namespace Amazonia.DeliveryRoute.RouteCalculation;

/// <summary>
/// Implements IRouteCalculator as a service
/// </summary>
public sealed record RouteCalculator
    : IRouteCalculator<Position>
{
    #region Cosntants
    private const int VerticeBuffer = 100;
    #endregion

    #region Properties
    private List<Vertex<Position>> WorkingSet { get; set; }

    private Vertex<Position>? Start { get; set; }

    private Vertex<Position>? Destination { get; set; }

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
    public async Task<IOrderedEnumerable<Position>> CalculateAsync(
        Grid<Position> grid,
        Position start,
        Position destination,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(grid);
        Guard.IsNotNull(start);
        Guard.IsNotNull(destination);

        this.CancellationToken = cancellationToken;

        if (start.Equals(destination))
        {
            throw new ArgumentException("Start position must be different", nameof(destination));
        }

        this.Start = grid.FindItem(start);
        this.Destination = grid.FindItem(destination);

        if (this.Start is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(start));
        }

        if (this.Destination is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(destination));
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
            this.WorkingSet.Clear();
            this.WorkingSet = new List<Vertex<Position>>(VerticeBuffer);

            this.Start!.TotalDistance = 0;

            this.IterateNeighbors(this.Start!);
        }, this.CancellationToken);
    }

    private void IterateNeighbors(Vertex<Position> vertex)
    {
        if (this.WorkingSet.Contains(vertex))
        {
            return;
        }

        this.WorkingSet.Add(vertex);

        foreach (var neighbor in vertex.Connections)
        {
            this.IterateNeighbors(neighbor.Other);
        }
    }
    #endregion

    private Task<Vertex<Position>> CalculateDistances()
    {
        return Task.Run(() =>
        {
            Vertex<Position>? result = null;

            while (this.WorkingSet.Count > 0)
            {
                var item = this.WorkingSet.OrderBy(i => i.TotalDistance).First();

                if (item.Equals(this.Destination))
                {
                    result = item;
                    break;
                }

                foreach (var neighbor in item.Connections)
                {
                    var calculatedDistance = item.TotalDistance + neighbor.Value;
                    if (calculatedDistance < neighbor.Other.TotalDistance)
                    {
                        neighbor.Other.TotalDistance = calculatedDistance;
                        neighbor.Other.Previous = item;

                        _ = this.WorkingSet.Remove(neighbor.Other);
                        this.WorkingSet.Add(neighbor.Other);
                    }
                }

                _ = this.WorkingSet.Remove(item);
            }

            return result!;
        }, this.CancellationToken);
    }

    private IOrderedEnumerable<Position> BuildPathToDestination(Vertex<Position>? vertex)
    {
        this.CancellationToken.ThrowIfCancellationRequested();
        var path = new List<Position>(VerticeBuffer);

        if (vertex is not null)
        {
            while (vertex is not null)
            {
                path.Add(vertex.Value);
                vertex = vertex.Previous;
            }
        }

        path.Reverse();
        return path.OrderBy(_ => 0);
    }
}
