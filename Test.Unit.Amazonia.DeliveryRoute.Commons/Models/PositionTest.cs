using Amazonia.DeliveryRoute.Commons.Models;
using System.ComponentModel.DataAnnotations;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Models;

public sealed record PositionTest
{
    #region Constants
    private const string ValidX = "A";

    private const int ValidY = 1;
    #endregion

    #region X Value
    [Theory]
    [InlineData("a")]
    [InlineData("A")]
    public void Position_X_IsValid(string xValue)
    {
        var model = new Position
        {
            X = xValue,
            Y = ValidY,
        };

        var validation = ValidateModel(model);
        Assert.Empty(validation);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("aa")]
    [InlineData("AA")]
    public void Position_X_IsNotValid(string xValue)
    {
        var model = new Position
        {
            X = xValue,
            Y = ValidY,
        };

        var validation = ValidateModel(model);
        var item = Assert.Single(validation);

        Assert.Contains(nameof(Position.X), item.MemberNames);
    }
    #endregion

    #region Y Value
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Position_Y_IsValid(int yValue)
    {
        var model = new Position
        {
            X = ValidX,
            Y = yValue,
        };

        var validation = ValidateModel(model);
        Assert.Empty(validation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(9)]
    public void Position_Y_IsNotValid(int yValue)
    {
        var model = new Position
        {
            X = ValidX,
            Y = yValue,
        };

        var validation = ValidateModel(model);
        var item = Assert.Single(validation);

        Assert.Contains(nameof(Position.Y), item.MemberNames);
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
        var itemA = new Position
        {
            X = xItemA,
            Y = yItemA
        };

        var itemB = new Position
        {
            X = xItemB,
            Y = yItemB
        };

        Assert.Equal(expected, itemA.CompareTo(itemB));
    }

    [Fact]
    public void CompareTo_Null_AlwaysPositive()
    {
        var itemA = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        Assert.True(itemA.CompareTo(null) > 0);
    }
    #endregion

    [Fact]
    public void ToString_IsCorrect()
    {
        const string expected = "A1";

        var item = new Position
        {
            X = ValidX,
            Y = ValidY,
        };

        Assert.Equal(expected, item.ToString());
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);

        _ = Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
