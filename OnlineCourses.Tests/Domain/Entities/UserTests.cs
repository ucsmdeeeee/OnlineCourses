using FluentAssertions;
using OnlineCourses.Domain.Entities;
using System;
using Xunit;

namespace OnlineCourses.Tests.Domain
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "John Doe";
            var email = "john.doe@example.com";
            var role = "Student";

            // Act
            var user = new User
            {
                Id = id,
                Name = name,
                Email = email,
                Role = role
            };

            // Assert
            user.Id.Should().Be(id);
            user.Name.Should().Be(name);
            user.Email.Should().Be(email);
            user.Role.Should().Be(role);
        }

        [Fact]
        public void UpdateUser_ShouldChangeNameAndRole()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Old Name",
                Role = "Student"
            };

            // Act
            user.Name = "New Name";
            user.Role = "Teacher";

            // Assert
            user.Name.Should().Be("New Name");
            user.Role.Should().Be("Teacher");
        }

        [Fact]
        public void Email_ShouldThrowException_WhenInvalid()
        {
            // Arrange
            var invalidEmail = "invalid-email";

            // Act
            Action act = () => User.ValidateEmail(invalidEmail);

            // Assert
            act.Should().Throw<FormatException>().WithMessage("Invalid email format");
        }



    }
}
