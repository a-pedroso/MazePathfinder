namespace MazePathfinder.Api.Endpoints.Mazes;

using MazePathfinder.Domain.Maze;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

public static class GetEndpoint
{
    public static IEndpointRouteBuilder MapGetEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", EndpointHandler)
           .WithName("GetMazes")
           .WithSummary("Get All Mazes (without pagination)");
        return app;
    }

    private static async Task<Results<Ok<ReadOnlyCollection<MazeDTO>>, BadRequest<ProblemDetails>>> EndpointHandler(
        HttpContext context,
        IMazeRepository mazeRepository,
        //[FromQuery] int? page, // for future pagination implementation
        //[FromQuery] int? limit,
        CancellationToken cancellationToken)
    {
        var result = await mazeRepository.GetMazesAsync(cancellationToken);
        var dto = result.Select(s => new MazeDTO(s.Id, s.Map, s.Solution, s.UsedAlgorithm.ToString())).ToList().AsReadOnly();
        return TypedResults.Ok(dto);
    }
}