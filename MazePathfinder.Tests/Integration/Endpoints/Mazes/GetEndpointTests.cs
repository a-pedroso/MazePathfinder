namespace MazePathfinder.Tests.Integration.Endpoints.Mazes;

using MazePathfinder.Api.Endpoints.Mazes;
using MazePathfinder.Tests.Integration.Core;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

public class GetEndpointTests : IClassFixture<WebAppFactoryFixtureGetEndpoints>
{
    private readonly string _url = MapEndpoints.BaseUrl;
    private readonly WebAppFactoryFixtureGetEndpoints _app;
    private readonly ITestOutputHelper _output;

    public GetEndpointTests(
        WebAppFactoryFixtureGetEndpoints appFixture,
        ITestOutputHelper output)
    {
        _output = output;
        _app = appFixture;
    }

    [Fact]
    public async Task GetEndpoint_ReturnsOkResult_WhenCalledWithValidParameters()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        // Act
        HttpResponseMessage responseMessage = await client.GetAsync($"{_url}");
        _output.WriteLine(responseMessage.ToJson());

        IReadOnlyList<MazeDTO>? result = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<MazeDTO>>();

        // Assert
        Assert.NotNull(responseMessage);
        Assert.True(responseMessage.IsSuccessStatusCode);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }
}
