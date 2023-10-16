using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Test.Unit.Amazonia.DeliveryRoute.GridMap.Fixtures;
public static class HttpClientHelper
{
    public static Mock<HttpMessageHandler> MockResults<T>(T response, HttpStatusCode responseCode = HttpStatusCode.OK)
    {
        var mockResponse = new HttpResponseMessage()
        {
            Content = new StringContent(JsonConvert.SerializeObject(response)),
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
}