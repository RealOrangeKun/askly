using Askly.Api.Shared.Contracts;
using Askly.Api.Shared.Extensions;
using Askly.Api.Shared.Filters;
using Carter;
using FluentResults;
using MediatR;

namespace Askly.Api.Features.Questions.AnswerQuestion;

public class AnswerQuestionCommandEndpoint : ICarterModule
{
    public record Request(string AnswerText);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/admin/questions/{id}", async (Guid id, Request request, ISender sender) =>
        {
            Result result = await sender.Send(new AnswerQuestionCommand(id, request.AnswerText));

            var response = result.ToApiResponse(
                successMessage: "Answer added to question",
                failureMessage: "Answer couldn't be added to question"
            );

            if (result.IsFailed)
            {
                return Results.NotFound(response);
            }

            return Results.Ok(response);
        })
        .AddEndpointFilter<ApiKeyAuthFilter>()
        .WithName("AnswerQuestion")
        .WithSummary("Answer a question")
        .WithDescription("Adds an answer to an existing question and marks it as answered")
        .WithOpenApi()
        .Produces<ApiResponse>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags(Tags.Questions, Tags.Admin);
    }
}
