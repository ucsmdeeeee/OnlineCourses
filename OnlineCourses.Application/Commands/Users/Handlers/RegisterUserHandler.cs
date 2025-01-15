using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace OnlineCourses.Application.Commands.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserHandler(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var passwordHash = _passwordHasher.HashPassword(command.Password);
            var user = new User(command.Name, command.Email, command.Role, passwordHash);
            var existingUser = await _repository.GetByEmailAsync(command.Email);
            if (existingUser != null)
            {
                throw new Exception($"User with email {command.Email} already exists.");
            }


            await _repository.AddAsync(user);
            return user.Id;
        }
    }
}