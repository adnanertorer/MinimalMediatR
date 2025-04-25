using MinimalMediatR.Core;

namespace MinimalMediatR.ExampleApp.Features.Users;

public class CreateUserCommand : IRequest<CreateUserResponse>
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public class CreateUserResponse
{
    public Guid Id { get; set; }
}