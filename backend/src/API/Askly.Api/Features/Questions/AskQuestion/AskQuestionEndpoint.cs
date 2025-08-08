using Carter;
using FluentResults;
using MediatR;
using Askly.Api.Shared.Contracts;
using Askly.Api.Shared.Extensions;
using Askly.Api.Shared.Caching;

namespace Askly.Api.Features.Questions.AskQuestion;

public sealed class AskQuestionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/questions/ask", async (string questionText, ISender sender) =>
        {
            var query = new AskQuestionQuery(questionText);

            Result<IEnumerable<QuestionResponse>> result = await sender.Send(query);

            var response = result.ToApiResponse(
                successMessage: "Similar questions found",
                failureMessage: "Failed to find similar questions");

            return response.IsSuccess
                ? Results.Ok(response)
                : Results.BadRequest(response);
        })
        .CacheOutput(CachePolicy.PolicyNames.QuestionSearch)
        .WithName("AskQuestion")
        .WithSummary("Find similar questions")
        .WithDescription("Find the 5 most similar answered questions using vector similarity search")
        .WithOpenApi()
        .Produces<ApiResponse<IEnumerable<QuestionResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<IEnumerable<QuestionResponse>>>(StatusCodes.Status400BadRequest)
        .WithTags(Tags.Questions);
    }
}
