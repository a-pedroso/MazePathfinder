namespace MazePathfinder.Api.Endpoints.Mazes;

using MazePathfinder.Domain.Maze;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class GetByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", EndpointHandler)
           .WithName("GetMazeById")
           .WithSummary("Get Maze by Id");
        return app;
    }

    private static async Task<Results<Ok<MazeDTO>, NotFound<ProblemDetails>, BadRequest<ProblemDetails>>> EndpointHandler(
        HttpContext context,
        IMazeRepository mazeRepository,
        [FromRoute] Guid id, 
        CancellationToken cancellationToken)
    {
        var result = await mazeRepository.GetMazeByIdAsync(id, cancellationToken);
        return result is null
            ? TypedResults.NotFound<ProblemDetails>(
                new ProblemDetails()
                {
                    Detail = "Maze not found.",
                })
            : TypedResults.Ok(new MazeDTO(result.Id, result.Map, result.Solution, result.UsedAlgorithm.ToString()));
    }
}