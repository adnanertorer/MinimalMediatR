using MinimalMediatR.Core;
using MinimalMediatR.Tests.Models;

namespace MinimalMediatR.Tests.Queries;

public class GetUserQuery : IRequest<User>
{
    public int Id { get; set; }
}