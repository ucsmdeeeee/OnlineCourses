using MediatR;

public record RegisterUserCommand(string Name, string Email, string Role, string Password) : IRequest<Guid>;
