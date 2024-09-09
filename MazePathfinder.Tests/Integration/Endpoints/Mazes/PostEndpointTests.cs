namespace MazePathfinder.Tests.Integration.Endpoints.Mazes;

using MazePathfinder.Api.Endpoints.Mazes;
using MazePathfinder.Domain.AlgorithmServices;
using MazePathfinder.Tests.Integration.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

public class PostEndpointTests : IClassFixture<WebAppFactoryFixturePostEndpoint>
{
    private readonly string _url = MapEndpoints.BaseUrl;
    private readonly WebAppFactoryFixturePostEndpoint _app;
    private readonly ITestOutputHelper _output;

    public PostEndpointTests(
        WebAppFactoryFixturePostEndpoint appFixture,
        ITestOutputHelper output)
    {
        _output = output;
        _app = appFixture;
    }

    [Fact]
    public async Task PostEndpoint_ReturnsCreatedResult_WhenCalledWithValidParameters()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        // Act
        HttpResponseMessage responseMessage = await client.PostAsJsonAsync(
            $"{_url}",
            new PostEndpoint.SubmitMazeRequest(
                "S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_",
                AlgorithmsEnum.BreadthFirstSearch));

        _output.WriteLine(responseMessage.ToJson());

        MazeDTO? result = await responseMessage.Content.ReadFromJsonAsync<MazeDTO>();

        // Assert
        Assert.NotNull(responseMessage);
        Assert.True(responseMessage.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.Created, responseMessage.StatusCode);
        Assert.NotNull(result);
        Assert.NotNull(result.Solution);
        Assert.NotNull(result.Map);
    }

    [Fact]
    public async Task PostEndpoint_ReturnsUnprocessableEntityResult_WhenCalledWithInvalidMap()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        // Act
        HttpResponseMessage responseMessage = await client.PostAsJsonAsync(
            $"{_url}",
            new PostEndpoint.SubmitMazeRequest(
                "S_________\n_XXXXXXXX_\nXXXG",
                AlgorithmsEnum.BreadthFirstSearch));

        _output.WriteLine(responseMessage.ToJson());

        ProblemDetails? result = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.NotNull(responseMessage);
        Assert.False(responseMessage.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, responseMessage.StatusCode);
        Assert.NotNull(result);
        Assert.Equal("Validation error", result.Title);
    }

    [Fact]
    public async Task PostEndpoint_ReturnsUnprocessableEntityResult_WhenCalledWithInvalidAlgorithm()
    {
        // Arrange
        var client = _app.CreateDefaultClient();

        // Create the request object with an invalid algorithm
        var request = new
        {
            map = "S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_",
            algorithm = -1
        };

        // Act
        HttpResponseMessage responseMessage = await client.PostAsJsonAsync(
            $"{_url}", request);
        
        _output.WriteLine(responseMessage.ToJson());

        ProblemDetails? result = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.NotNull(responseMessage);
        Assert.False(responseMessage.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, responseMessage.StatusCode);
        Assert.NotNull(result);
        Assert.Equal("Unprocessable Entity", result.Title);
    }
}