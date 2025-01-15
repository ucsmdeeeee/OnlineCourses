using AutoMapper;
using FluentAssertions;
using Moq;
using OnlineCourses.Application.DTOs.Users;
using OnlineCourses.Application.Queries;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Application.Users.Queries.Handlers
{
    public class GetUserByIdHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly GetUserByIdHandler _handler;

        public GetUserByIdHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new GetUserByIdHandler(_mockUserRepository.Object); // Передайте только один аргумент, если конструктор ожидает один
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User
            {
                Id = userId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Role = "Student"
            };

            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var query = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new GetUserByIdDto
            {
                Id = userId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Role = "Student"
            });
        }
    





    [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
