using Amazonia.DeliveryRoute.GridMap;
using Amazonia.DeliveryRoute.GridMap.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Text.Json.Nodes;
using Test.Unit.Amazonia.DeliveryRoute.GridMap.Fixtures;

namespace Test.Unit.Amazonia.DeliveryRoute.GridMap;

public sealed record GridServiceTest
{
    #region Constants
    private const string ApiUri = "/test";

    private const string BaseUri = "https://localhost/";
    #endregion

    #region Properties
    private Mock<IOptions<GridMapOptions>> OptionsMock { get; }

    private Mock<ILogger<IGridService>> LogMock { get; }
    #endregion

    #region Constructors
    public GridServiceTest()
    {
        this.OptionsMock = new Mock<IOptions<GridMapOptions>>();
        this.LogMock = new Mock<ILogger<IGridService>>();

        var optionValues = new GridMapOptions
        {
            GridSourceApiUri = ApiUri,
            GridSourceBaseUri = BaseUri,
        };

        _ = this.OptionsMock
            .Setup(m => m.Value)
            .Returns(optionValues);
    }
    #endregion

    #region IDisposable
    [Fact]
    public async Task AcquireData_CallsApi()
    {
        var baseUri = new Uri(BaseUri);
        var result = new JsonObject();

        var handlerMock = HttpClientHelper.MockResults(result);
        using var clientMock = new HttpClient(handlerMock.Object);
        clientMock.BaseAddress = baseUri;

        var subject = new GridService(
            this.OptionsMock.Object,
            this.LogMock.Object,
            clientMock);

        _ = await subject.BuildGridAsync();

        handlerMock
            .Protected()
            .Verify("SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => HttpMethod.Get.Equals(req.Method)
                    && new Uri(baseUri, ApiUri).Equals(req.RequestUri)),
                ItExpr.IsAny<CancellationToken>());
    }
    #endregion
}