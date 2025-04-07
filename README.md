# MinimalMediatR

MinimalMediatR is a lightweight and extensible implementation of MediatR.

## ✨ Features
- Send/Receive pattern (`IRequest<T>` + `IRequestHandler<T, R>`)  
- Publish/Subscribe event model (`INotification` + `INotificationHandler<T>`)  
- Middleware support via `IPipelineBehavior<TRequest, TResponse>`
- Auto-registration with `AddMinimalMediatR(...)`

## 🚀 Quick Start

```csharp
services.AddMinimalMediatR(typeof(MyHandler).Assembly);
```

## 🧱 Sample Query

```csharp
public class GetUserQuery : IRequest<User> 
{ 
    public int Id { get; set; } 
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(new User { Id = request.Id, Name = $"User {request.Id}" });
}
```

## 🔔 Sample Notification

```csharp
public class UserCreatedEvent : INotification
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User created: {notification.Name}");
        return Task.CompletedTask;
    }
}
```

## 📦 License

MIT License


// LICENSE
MIT License

Copyright (c) 2025 Adnan Ertorer

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
