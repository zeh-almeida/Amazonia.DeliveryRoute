﻿using Amazonia.DeliveryRoute.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Test.Unit.Amazonia.DeliveryRoute.Api.Models;

public sealed record PointOfInterestTest
{
    #region Constants
    private const string ValidX = "A";

    private const int ValidY = 1;

    private static Position ValidPosition { get; } = new Position
    {
        X = ValidX,
        Y = ValidY,
    };
    #endregion

    #region InterestType
    [Theory]
    [MemberData(nameof(InterestTypeValues))]
    public void InterestType_Set_IsValid(InterestType value)
    {
        var model = new PointOfInterest
        {
            InterestType = value,
            Position = ValidPosition,
        };

        var validation = ValidateModel(model);
        Assert.Empty(validation);
    }

    [Fact]
    public void InterestType_NotSet_IsNotValid()
    {
        var model = new PointOfInterest
        {
            InterestType = null,
            Position = ValidPosition,
        };

        var validation = ValidateModel(model);
        var item = Assert.Single(validation);

        Assert.Contains(nameof(PointOfInterest.InterestType), item.MemberNames);
    }
    #endregion

    #region Equality
    [Fact]
    public void Equals_Null_False()
    {
        var itemA = new PointOfInterest
        {
            InterestType = InterestType.ObjectPickup,
            Position = ValidPosition,
        };

        Assert.False(itemA.Equals(null));
    }

    [Fact]
    public void Equals_DifferentType_False()
    {
        var itemA = new PointOfInterest
        {
            InterestType = InterestType.ObjectPickup,
            Position = ValidPosition,
        };

        Assert.False(itemA.Equals(0));
    }

    [Fact]
    public void Equals_SamePosition_True()
    {
        var itemA = new PointOfInterest
        {
            InterestType = InterestType.ObjectPickup,
            Position = ValidPosition,
        };

        var itemB = new PointOfInterest
        {
            InterestType = InterestType.ObjectPickup,
            Position = ValidPosition,
        };

        Assert.True(itemA.Equals(itemB));
    }

    [Fact]
    public void HashCode_Calculates()
    {
        var itemA = new PointOfInterest
        {
            InterestType = InterestType.ObjectPickup,
            Position = ValidPosition,
        };

        var expected = HashCode.Combine(InterestType.ObjectPickup, ValidPosition);
        Assert.Equal(expected, itemA.GetHashCode());
    }
    #endregion

    #region Comparisons
    [Theory]
    [InlineData("A", 1, "A", 1, 0)]
    [InlineData("A", 1, "A", 2, -1)]
    [InlineData("A", 2, "A", 1, 1)]
    [InlineData("B", 1, "A", 1, 1)]
    [InlineData("A", 1, "B", 1, -1)]
    public void CompareTo_Executes(
        string xItemA, int yItemA,
        string xItemB, int yItemB,
        int expected)
    {
        var itemA = new PointOfInterest
        {
            InterestType = InterestType.OriginPosition,
            Position = new Position
            {
                X = xItemA,
                Y = yItemA
            },
        };

        var itemB = new PointOfInterest
        {
            InterestType = InterestType.OriginPosition,
            Position = new Position
            {
                X = xItemB,
                Y = yItemB
            },
        };

        Assert.Equal(expected, itemA.CompareTo(itemB));
    }
    #endregion

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);

        _ = Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    public static IEnumerable<object[]> InterestTypeValues()
    {
        foreach (var value in Enum.GetValues<InterestType>())
        {
            yield return new object[] { value };
        }
    }
}
