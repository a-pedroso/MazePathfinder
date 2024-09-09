namespace MazePathfinder.Api.ProgramExtensions;

using FluentValidation.Results;
using Microsoft.AspNetCore.WebUtilities;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;
using SharpGrip.FluentValidation.AutoValidation.Shared.Extensions;

public static class FluentValidationAutoValidationExtension
{
    public static IServiceCollection AddFluentValidationAutoValidationExtension(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        //builder.Services.AddFluentValidationAutoValidation();
        return services.AddFluentValidationAutoValidation(configuration =>
        {
            // Replace the default result factory with a custom implementation.
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
    }

    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IResult CreateResult(EndpointFilterInvocationContext context, ValidationResult validationResult)
        {
            return context.HttpContext.Request.Method switch
            {
                "POST" or "PUT" or "PATCH"
                  => Results.ValidationProblem(
                                    statusCode: StatusCodes.Status422UnprocessableEntity,
                                    type: ProblemDetailsType.UnprocessableEntity,
                                    title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status422UnprocessableEntity),
                                    errors: validationResult.ToValidationProblemErrors(),
                                    detail: "See the 'errors' property for details."),

                _ => Results.ValidationProblem(
                                    statusCode: StatusCodes.Status400BadRequest,
                                    type: ProblemDetailsType.BadRequest,
                                    title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                                    errors: validationResult.ToValidationProblemErrors(),
                                    detail: "See the 'errors' property for details."),
            };
        }
    }
}

