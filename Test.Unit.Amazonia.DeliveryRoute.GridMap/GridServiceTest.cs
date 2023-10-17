using Amazonia.DeliveryRoute.GridMap;
using Amazonia.DeliveryRoute.GridMap.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Test.Unit.Amazonia.DeliveryRoute.GridMap.Fixtures;
using Test.Unit.Amazonia.DeliveryRoute.GridMap.Resources;

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

    #region AcquireData
    [Fact]
    public async Task AcquireData_CallsApi()
    {
        var baseUri = new Uri(BaseUri);
        var result = new JsonObject();

        var handlerMock = HttpClientHelper.MockResults(result);
        using var clientMock = MockClient(handlerMock);

        var subject = new GridService(
            this.OptionsMock.Object,
            this.LogMock.Object,
            clientMock);

        var grid = await subject.BuildGridAsync();

        handlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => HttpMethod.Get.Equals(req.Method)
                    && new Uri(baseUri, ApiUri).Equals(req.RequestUri)),
                ItExpr.IsAny<CancellationToken>());

        Assert.NotNull(grid);
        Assert.Empty(grid.AsEnumerable());
    }

    [Fact]
    public async Task AcquireData_OnError_EmptyGrid()
    {
        var handlerMock = HttpClientHelper.MockException();
        using var clientMock = MockClient(handlerMock);

        var subject = new GridService(
            this.OptionsMock.Object,
            this.LogMock.Object,
            clientMock);

        var grid = await subject.BuildGridAsync();

        handlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());

        this.LogMock
            .Verify(
                m => m.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((_, @type) => @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

        Assert.NotNull(grid);
        Assert.Empty(grid.AsEnumerable());
    }

    [Fact]
    public async Task AcquireData_WithData_BuildsGrid()
    {
        var baseUri = new Uri(BaseUri);

        using var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(TestResources.ValidGridJson));

        var result = await JsonSerializer.DeserializeAsync<JsonObject>(
            dataStream!,
            new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,

            }
            );

        var handlerMock = HttpClientHelper.MockResults(result);
        using var clientMock = MockClient(handlerMock);

        var subject = new GridService(
            this.OptionsMock.Object,
            this.LogMock.Object,
            clientMock);

        var grid = await subject.BuildGridAsync();

        handlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => HttpMethod.Get.Equals(req.Method)
                    && new Uri(baseUri, ApiUri).Equals(req.RequestUri)),
                ItExpr.IsAny<CancellationToken>());

        Assert.NotNull(grid);
        Assert.Equal(64, grid.AsEnumerable().Count());
    }
    #endregion

    private static HttpClient MockClient(Mock<HttpMessageHandler> handlerMock)
    {
        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(BaseUri)
        };
    }
}