using App.Application.Abstractions.Messaging;
using App.Application.Todos;
using App.Application.Todos.GetById;
using App.Domain;
using Microsoft.AspNetCore.Mvc;

namespace App.Presentation.Endpoints.Todos;

internal sealed class TodoGetByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("todos/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IQueryHandler<GetTodoByIdQuery, GetTodoResponse> handler,
            CancellationToken cancellationToken
        ) => {
            GetTodoByIdQuery command = new(id);

            Result<GetTodoResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Todos)
        .RequireAuthorization()
        .Produces<GetTodoResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
