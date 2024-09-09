namespace MazePathfinder.Tests.Integration.Endpoints.Mazes;

using MazePathfinder.Api.Endpoints.Mazes;
using MazePathfinder.Domain.AlgorithmServices;
using MazePathfinder.Tests.Integration.Core;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static MazePathfinder.Api.Endpoints.Mazes.PostEndpoint;

public class GetByIdEndpointTests : IClassFixture<WebAppFactoryFixtureGetEndpoints>
{
    private readonly string _url = MapEndpoints.BaseUrl;
    private readonly WebAppFactoryFixtureGetEndpoints _app;
    private readonly ITestOutputHelper _output;

    public GetByIdEndpointTests(
        WebAppFactoryFixtureGetEndpoints appFixture,
        ITestOutputHelper output)
    {
        _output = output;
        _app = appFixture;
    }

    [Fact]
    public async Task GetByIdEndpoint_ReturnsOkResult_WhenCalledWithValidParameters()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        HttpResponseMessage postResponseMessage = await client.PostAsJsonAsync(
            $"{_url}",
            new SubmitMazeRequest(
                "S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_",
                AlgorithmsEnum.BreadthFirstSearch));

        MazeDTO? postDto = await postResponseMessage.Content.ReadFromJsonAsync<MazeDTO>();
        Guid mazeId = postDto.Id;

        // Act
        HttpResponseMessage responseMessage = await client.GetAsync($"{_url}/{mazeId}");
        _output.WriteLine(responseMessage.ToJson());

        MazeDTO? result = await responseMessage.Content.ReadFromJsonAsync<MazeDTO>();

        // Assert
        Assert.NotNull(responseMessage);
        Assert.True(responseMessage.IsSuccessStatusCode);
        Assert.NotNull(result);
        Assert.Equal(mazeId, result.Id);
    }

    [Fact]
    public async Task GetByIdEndpoint_ReturnsNotFoundResult_WhenCalledWithInvalidParameters()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        // Act
        HttpResponseMessage responseMessage = await client.GetAsync($"{_url}/{Guid.NewGuid()}");
        _output.WriteLine(responseMessage.ToJson());

        // Assert
        Assert.NotNull(responseMessage);
        Assert.False(responseMessage.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    [Fact]
    public async Task GetByIdEndpoint_ReturnsBadRequestResult_WhenCalledWithInvalidParameters()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        // Act
        HttpResponseMessage responseMessage = await client.GetAsync($"{_url}/invalid-guid");
        _output.WriteLine(responseMessage.ToJson());

        // Assert
        Assert.NotNull(responseMessage);
        Assert.False(responseMessage.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, responseMessage.StatusCode);
    }
}
