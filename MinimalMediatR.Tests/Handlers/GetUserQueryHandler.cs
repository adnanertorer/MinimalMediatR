using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Tests.Infrastructure;
using MinimalMediatR.Tests.Models;
using MinimalMediatR.Tests.Queries;

namespace MinimalMediatR.Tests.Handlers;

public class GetUserQueryHandler(AppDbContext context) : IRequestHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await context.Users.FirstAsync(u => u.Id == request.Id, cancellationToken);
    }
}