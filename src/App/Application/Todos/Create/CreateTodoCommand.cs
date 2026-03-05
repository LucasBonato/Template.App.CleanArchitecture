using App.Application.Abstractions.Messaging;
using App.Domain.Todos;

namespace App.Application.Todos.Create;

public sealed record CreateTodoCommand(
    Guid UserId,
    string Description,
    DateTime? DueDate,
    List<string> Labels,
    Priority Priority
) : ICommand<Guid>;
