using Askly.Api.Shared.Contracts;
using Askly.Api.Shared.Extensions;
using Askly.Api.Shared.Filters;
using Carter;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Askly.Api.Features.Questions.GetQuestions;

public sealed class GetQuestionsEndpoint : ICarterModule
{
    public sealed record QueryParameters([FromQuery] int PageNumber = 0, [FromQuery] int PageSize = 10, [FromQuery] bool Answered = false);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/questions/", async ([AsParameters] QueryParameters parameters, ISender sender) =>
        {
            var query = new GetQuestionsQuery(parameters.PageNumber, parameters.PageSize, parameters.Answered);

            Result<IEnumerable<QuestionResponse>> result = await sender.Send(query);

            var response = result.ToApiResponse(
                successMessage: "Questions retrieved successfully",
                failureMessage: "Failed to retrieve questions");

            return response.IsSuccess
                ? Results.Ok(response)
                : Results.BadRequest(response);
        })
        .AddEndpointFilter<ApiKeyAuthFilter>()
        .WithName("GetQuestions")
        .WithSummary("Get questions")
        .WithDescription("Retrieves a paginated list of questions. Use 'answered' parameter to filter by status.")
        .WithOpenApi()
        .Produces<ApiResponse<IEnumerable<QuestionResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<IEnumerable<QuestionResponse>>>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags(Tags.Questions, Tags.Admin);
    }
}
