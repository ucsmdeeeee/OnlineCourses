using FluentAssertions;
using Moq;
using OnlineCourses.Application.Commands;
using OnlineCourses.Application.Commands.Handlers;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Application.Users.Commands.Handlers
{
    public class LoginUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly LoginUserHandler _handler;

        public LoginUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new LoginUserHandler(_userRepositoryMock.Object, _passwordHasherMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginUserCommand("john.doe@example.com", "password123");
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                PasswordHash = "hashed-password"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(command.Password, user.PasswordHash)).Returns(true);
            _tokenServiceMock.Setup(service => service.GenerateToken(user)).Returns("jwt-token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("jwt-token");
        }
    }
}
