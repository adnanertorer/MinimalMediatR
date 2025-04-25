using MinimalMediatR.Core;

namespace MinimalMediatR.ExampleApp.Features.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Handler] Creating user {request.Username} with email {request.Email}");

        return await Task.FromResult(new CreateUserResponse
        {
            Id = Guid.NewGuid()
        });
    }
}