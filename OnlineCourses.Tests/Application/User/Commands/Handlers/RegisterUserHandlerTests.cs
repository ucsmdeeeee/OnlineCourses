using AutoMapper;
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
    public class RegisterUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterUserHandler _handler;
        public RegisterUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new RegisterUserHandler(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new RegisterUserCommand("John Doe", "john.doe@example.com", "password123", "Student");

            _userRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => user.Id = userId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(userId);
        }



        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                Role = "Student"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(existingUser.Email))
                .ReturnsAsync(existingUser);

            var command = new RegisterUserCommand("John Doe", "john.doe@example.com", "password123", "Student");

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"User with email {existingUser.Email} already exists.");
        }

    }
}
