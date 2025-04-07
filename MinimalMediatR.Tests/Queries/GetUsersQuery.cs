using MinimalMediatR.Core;
using MinimalMediatR.Tests.Models;

namespace MinimalMediatR.Tests.Queries;

public class GetUsersQuery : IRequest<List<User>>
{
    
}