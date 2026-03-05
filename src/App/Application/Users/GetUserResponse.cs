namespace App.Application.Users;

public sealed record GetUserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName
);
