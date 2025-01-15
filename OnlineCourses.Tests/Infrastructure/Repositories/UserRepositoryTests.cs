using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Infrastructure.Persistence;
using OnlineCourses.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Infrastructure.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public UserRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                Role = "Student",
                PasswordHash = "hashedPassword"
            };

            // Act
            await repository.AddAsync(user);
            var result = await repository.GetByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("John Doe");
            result.Email.Should().Be("john.doe@example.com");
            result.Role.Should().Be("Student");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Role = "Teacher",
                PasswordHash = "hashedPassword"
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var retrievedUser = await repository.GetByIdAsync(user.Id);

            // Assert
            retrievedUser.Should().NotBeNull();
            retrievedUser.Name.Should().Be("Jane Doe");
            retrievedUser.Email.Should().Be("jane.doe@example.com");
            retrievedUser.Role.Should().Be("Teacher");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var repository = new UserRepository(context);

            // Act
            var retrievedUser = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            retrievedUser.Should().BeNull();
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "existing.email@example.com",
                Role = "Student",
                PasswordHash = "hashedPassword"
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var emailExists = await repository.EmailExistsAsync("existing.email@example.com");

            // Assert
            emailExists.Should().BeTrue();
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var repository = new UserRepository(context);

            // Act
            var emailExists = await repository.EmailExistsAsync("nonexistent.email@example.com");

            // Assert
            emailExists.Should().BeFalse();
        }
    }
}
