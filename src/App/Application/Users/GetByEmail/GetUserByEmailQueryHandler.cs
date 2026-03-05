using App.Application.Abstractions.Authentication;
using App.Application.Abstractions.Data;
using App.Application.Abstractions.Messaging;
using App.Domain;
using App.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext
) : IQueryHandler<GetUserByEmailQuery, GetUserResponse> {
    public async Task<Result<GetUserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        GetUserResponse? user = await context.Users
            .Where(user => user.Email == query.Email)
            .Select(user =>
                new GetUserResponse(
                    Id: user.Id,
                    Email: user.Email,
                    FirstName: user.FirstName,
                    LastName: user.LastName
                )
            )
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure<GetUserResponse>(UserErrors.NotFoundByEmail);

        if (user.Id != userContext.UserId)
            return Result.Failure<GetUserResponse>(UserErrors.Unauthorized());

        return user;
    }
}
