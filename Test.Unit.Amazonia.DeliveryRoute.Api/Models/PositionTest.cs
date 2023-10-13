using Amazonia.DeliveryRoute.Api.Models;
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

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);

        _ = Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
