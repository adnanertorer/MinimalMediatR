using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Tests.Infrastructure;
using MinimalMediatR.Tests.Models;
using MinimalMediatR.Tests.Queries;

namespace MinimalMediatR.Tests.Handlers;

public class GetUsersQueryHandler(AppDbContext context) : IRequestHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await context.Users.ToListAsync(cancellationToken);
        return users;
    }
}