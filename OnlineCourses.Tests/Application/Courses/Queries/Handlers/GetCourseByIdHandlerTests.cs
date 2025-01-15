using FluentAssertions;
using Moq;
using OnlineCourses.Application.DTOs.Courses;
using OnlineCourses.Application.Queries;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Application.Courses.Queries.Handlers
{
    public class GetCourseByIdHandlerTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly GetCourseByIdHandler _handler;

        public GetCourseByIdHandlerTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _handler = new GetCourseByIdHandler(_courseRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCourse_WhenCourseExists()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var course = new Course
            {
                Id = courseId,
                Title = "Test Title",
                Description = "Test Description",
                Price = 100
            };

            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(courseId))
                .ReturnsAsync(course);

            // Act
            var result = await _handler.Handle(new GetCourseByIdQuery(courseId), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(new GetCourseByIdDto
            {
                Id = courseId,
                Title = "Test Title",
                Description = "Test Description",
                Price = 100
            });
        }


        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCourseDoesNotExist()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(courseId))
                .ReturnsAsync((Course)null);

            // Act
            var result = await _handler.Handle(new GetCourseByIdQuery(courseId), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
