namespace MazePathfinder.Api.Endpoints.Mazes;

using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

public static class MapEndpoints
{
    public const string BaseUrl = "/mazes";

    public static WebApplication MapMazesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(BaseUrl)
                       .WithTags("Mazes")
                       .WithOpenApi()
                       .AddFluentValidationAutoValidation();

        group.MapGetEndpoint()
             .MapPostEndpoint()
             .MapGetByIdEndpoint();

        return app;
    }
}