using App.Application.Abstractions.Authentication;
using App.Application.Abstractions.Data;
using App.Application.Abstractions.Messaging;
using App.Domain;
using App.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext
) : IQueryHandler<GetUserByIdQuery, GetUserResponse> {
    public async Task<Result<GetUserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
            return Result.Failure<GetUserResponse>(UserErrors.Unauthorized());

        GetUserResponse? user = await context.Users
            .Where(user => user.Id == query.UserId)
            .Select(user =>
                new GetUserResponse(
                    Id: user.Id,
                    Email: user.Email,
                    FirstName: user.FirstName,
                    LastName: user.LastName
                )
            )
            .SingleOrDefaultAsync(cancellationToken);

        return user ?? Result.Failure<GetUserResponse>(UserErrors.NotFound(query.UserId));
    }
}
