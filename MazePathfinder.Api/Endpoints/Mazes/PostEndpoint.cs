namespace MazePathfinder.Api.Endpoints.Mazes;

using FluentValidation;
using MazePathfinder.Domain.AlgorithmServices;
using MazePathfinder.Domain.Common;
using MazePathfinder.Domain.Maze;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public static class PostEndpoint
{
    public static IEndpointRouteBuilder MapPostEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", EndpointHandler)
           .WithName("SubmitNewMaze")
           .WithSummary("Submit new maze and get a possible solution")
           .WithDescription($"List of available algorithms:<br/>{GetListOfAvailableAlgorithms()}");

        return app;
    }

    private static async Task<Results<Created<MazeDTO>, BadRequest<ProblemDetails>, UnprocessableEntity<ProblemDetails>>> EndpointHandler(
        IMazeRepository mazeRepository,
        [FromBody] SubmitMazeRequest request,
        CancellationToken cancellationToken)
    {
        Result<MazeEntity> result = MazeEntity.CreateEntity(request.Map);

        if (result.IsFailure)
        {
            return TypedResults.UnprocessableEntity(new ProblemDetails
            {
                Title = "Validation error",
                Detail = string.Join(", ", result.Errors),
            });
        }

        MazeEntity entity = result.Value!;

        bool mazeSolved = entity.SolveMaze(request.Algorithm);

        await mazeRepository.AddMazeAsync(entity, cancellationToken);

        // TODO: to validate the requirement of returning a error when no solution is found
        //if (!mazeSolved)
        //{
        //    return TypedResults.UnprocessableEntity(new ProblemDetails
        //    {
        //        Title = "No solution found",
        //        Detail = "No solution found",
        //    });
        //}

        return TypedResults.Created($"{MapEndpoints.BaseUrl}/{entity.Id}", new MazeDTO(entity.Id, entity.Map, entity.Solution, entity.UsedAlgorithm.ToString()));
    }

    public record SubmitMazeRequest(
        string Map,
        AlgorithmsEnum Algorithm);

    public class SubmitMazeRequestValidator : AbstractValidator<SubmitMazeRequest>
    {
        public SubmitMazeRequestValidator()
        {
            RuleFor(x => x.Map)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Algorithm)
                 .IsInEnum()
                     .WithMessage("must be a valid algorithm option");
        }
    }

    private static string GetListOfAvailableAlgorithms()
    {
        var enumValues = Enum.GetValues<AlgorithmsEnum>().Cast<int>();
        var enumNames = Enum.GetNames<AlgorithmsEnum>();
        return string.Join("<br/>", enumValues.Zip(enumNames, (value, name) => $"{value} - {name}"));
    }
}
