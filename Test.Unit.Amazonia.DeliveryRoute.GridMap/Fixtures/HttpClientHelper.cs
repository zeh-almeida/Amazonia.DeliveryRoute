using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.Unit.Amazonia.DeliveryRoute.GridMap.Fixtures;
public static class HttpClientHelper
{
    #region Constants
    public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,

    };
    #endregion

    public static Mock<HttpMessageHandler> MockResults<T>(T response, HttpStatusCode responseCode = HttpStatusCode.OK)
    {
        var stream = new MemoryStream();
        JsonSerializer.Serialize<object>(stream, response!, Options);

        var mockResponse = new HttpResponseMessage()
        {
            Content = new StringContent(Encoding.UTF8.GetString(stream.ToArray())),
            StatusCode = responseCode
        };

        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse)
            .Verifiable();

        return mockHandler;
    }

    public static Mock<HttpMessageHandler> MockException()
    {
        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException())
            .Verifiable();

        return mockHandler;
    }
}