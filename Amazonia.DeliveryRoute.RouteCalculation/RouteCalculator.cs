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
    private HashSet<Vertice<GridItem>> Calculated { get; set; }

    private PriorityQueue<Vertice<GridItem>, decimal> WorkingSet { get; set; }

    private Grid? CurrentGrid { get; set; }

    private Position? Start { get; set; }

    private Position? Destination { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new RouteCalculator
    /// </summary>
    public RouteCalculator()
    {
        this.Calculated = [];
        this.WorkingSet = new PriorityQueue<Vertice<GridItem>, decimal>();
    }
    #endregion

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IOrderedEnumerable<GridItem>> Calculate(Grid grid, Position start, Position destination)
    {
        Guard.IsNotNull(grid);
        Guard.IsNotNull(start);
        Guard.IsNotNull(destination);

        if (grid.FindItem(start) is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(start));
        }

        if (grid.FindItem(destination) is null)
        {
            throw new ArgumentException("Position not found at grid", nameof(destination));
        }

        this.Start = start;
        this.CurrentGrid = grid;
        this.Destination = destination;

        await this.PrepareVertices();
        await this.CalculateDistances();

        return this.BuildPathToDestination();
    }

    #region Preparation
    private Task PrepareVertices()
    {
        return Task.Run(() =>
        {
            var items = this.CurrentGrid.AsEnumerable();
            var itemCount = items.Count();

            this.WorkingSet.Clear();
            this.WorkingSet = new PriorityQueue<Vertice<GridItem>, decimal>(itemCount);

            this.Calculated.Clear();
            this.Calculated = new HashSet<Vertice<GridItem>>(itemCount);

            foreach (var item in items)
            {
                var added = this.AddToWorkingSet(item);

                if (added is null)
                {
                    continue;
                }

                foreach (var neighbor in item.AllNeighbors())
                {
                    var neighborVertice = this.AddToWorkingSet(neighbor);

                    if (neighborVertice is not null)
                    {
                        added.Neighbors.Add(neighborVertice);
                    }
                }
            }
        });
    }

    private Vertice<GridItem>? AddToWorkingSet(GridItem gridItem)
    {
        var vertice = new Vertice<GridItem>
        {
            Value = gridItem,
        };

        if (this.WorkingSet.UnorderedItems.Contains((vertice, vertice.Distance)))
        {
            return null;
        }

        if (gridItem.Position.Equals(this.Start))
        {
            vertice.Distance = 0;
        }

        this.WorkingSet.Enqueue(vertice, vertice.Distance);
        return vertice;
    }
    #endregion

    private Task CalculateDistances()
    {
        return Task.Run(() =>
        {
            while (this.WorkingSet.Count > 0)
            {
                var item = this.WorkingSet.Dequeue();
                _ = this.Calculated.Add(item);

                foreach (var neighbor in item.Neighbors)
                {
                    var calculatedDistance = item.Distance + (neighbor.Distance - item.Distance);
                    if (calculatedDistance < neighbor.Distance)
                    {
                        neighbor.Distance = calculatedDistance;

                        if (!this.Calculated.Contains(neighbor))
                        {
                            this.WorkingSet.Enqueue(neighbor, neighbor.Distance);
                        }
                    }
                }
            }
        });
    }

    private IOrderedEnumerable<GridItem> BuildPathToDestination()
    {
        var items = this.Calculated.OrderBy(i => i.Distance).ToArray();
        var finalPath = new List<GridItem>(items.Length);

        foreach (var item in items)
        {
            finalPath.Add(item.Value);

            if (item.Value.Equals(this.Destination))
            {
                break;
            }
        }

        return finalPath.OrderBy(key => 0);
    }
}
