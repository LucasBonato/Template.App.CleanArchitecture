using App.Application.Abstractions.Messaging;
using App.Application.Todos;
using App.Application.Todos.Get;
using App.Domain;
using Microsoft.AspNetCore.Mvc;

namespace App.Presentation.Endpoints.Todos;

internal sealed class TodoGetEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("todos", async (
            [FromQuery] Guid userId,
            [FromServices] IQueryHandler<GetTodosQuery, List<GetTodoResponse>> handler,
            CancellationToken cancellationToken
        ) => {
            GetTodosQuery query = new(userId);

            Result<List<GetTodoResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Todos)
        .RequireAuthorization()
        .Produces<List<GetTodoResponse>>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
