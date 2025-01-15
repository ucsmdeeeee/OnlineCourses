using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineCourses.Domain.Interfaces;
namespace OnlineCourses.Application.Commands.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(IUserRepository repository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Неверный email или пароль.");
            }

            return _tokenService.GenerateToken(user);
        }
    }
}
