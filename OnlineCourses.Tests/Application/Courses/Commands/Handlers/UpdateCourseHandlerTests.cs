using FluentAssertions;
using MediatR;
using Moq;
using OnlineCourses.Application.Commands;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Application.Courses.Commands.Handlers
{
    public class UpdateCourseHandlerTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly UpdateCourseHandler _handler;

        public UpdateCourseHandlerTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _handler = new UpdateCourseHandler(_courseRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCourse_WhenCourseExists()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var command = new UpdateCourseCommand(courseId, "Updated Title", "Updated Description", 200m);
            var existingCourse = new Course
            {
                Id = courseId,
                Title = "Old Title",
                Description = "Old Description",
                Price = 100m
            };

            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(courseId))
                .ReturnsAsync(existingCourse);

            _courseRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Course>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _courseRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Course>(c =>
                c.Id == courseId &&
                c.Title == command.Title &&
                c.Description == command.Description &&
                c.Price == command.Price)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCourseDoesNotExist()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var command = new UpdateCourseCommand(courseId, "Updated Title", "Updated Description", 200m);

            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(courseId))
                .ReturnsAsync((Course)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
    .WithMessage($"Course with Id {courseId} does not exist.");

        }
    }
}
