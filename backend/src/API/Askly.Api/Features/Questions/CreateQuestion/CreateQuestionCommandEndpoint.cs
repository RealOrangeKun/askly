using Carter;
using FluentResults;
using MediatR;
using Askly.Api.Shared.Contracts;
using Askly.Api.Shared.Extensions;

namespace Askly.Api.Features.Questions.CreateQuestion;

public sealed class CreateQuestionCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/questions", async (CreateQuestionCommand command, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(command);
            var response = result.ToApiResponse(
                successMessage: "Question created successfully",
                failureMessage: "Failed to create question");

            return response.IsSuccess
                ? Results.Created($"/api/questions/{result.Value}", response)
                : Results.BadRequest(response);
        })
        .WithName("CreateQuestion")
        .WithSummary("Create a new question")
        .WithDescription("Creates a new question with automatic embedding generation")
        .WithOpenApi()
        .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status400BadRequest)
        .WithTags(Tags.Questions);
    }
}
