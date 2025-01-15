using FluentAssertions;
using OnlineCourses.Domain.Entities;
using System;
using Xunit;

namespace OnlineCourses.Tests.Domain
{
    public class CourseTests
    {
        [Fact]
        public void CreateCourse_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var title = "Test Course";
            var description = "Test Description";
            var price = 100m;

            // Act
            var course = new Course
            {
                Id = id,
                Title = title,
                Description = description,
                Price = price
            };

            // Assert
            course.Id.Should().Be(id);
            course.Title.Should().Be(title);
            course.Description.Should().Be(description);
            course.Price.Should().Be(price);
        }

        [Fact]
        public void UpdateCourse_ShouldChangeTitleAndDescription()
        {
            // Arrange
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Old Title",
                Description = "Old Description"
            };

            // Act
            course.Title = "New Title";
            course.Description = "New Description";

            // Assert
            course.Title.Should().Be("New Title");
            course.Description.Should().Be("New Description");
        }

        [Fact]
        public void Price_ShouldThrowException_WhenNegative()
        {
            // Act
            Action act = () => new Course { Price = -1 };

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Price cannot be negative.");
        }

    }
}
