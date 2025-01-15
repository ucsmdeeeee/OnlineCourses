using MediatR;

public record LoginUserCommand(string Email, string Password) : IRequest<string>; // Возвращает токен
