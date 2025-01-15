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
    public class DeleteCourseHandlerTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly DeleteCourseHandler _handler;

        public DeleteCourseHandlerTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _handler = new DeleteCourseHandler(_courseRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCourse_WhenCourseExists()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            _courseRepositoryMock
                .Setup(repo => repo.DeleteAsync(courseId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new DeleteCourseCommand(courseId), CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _courseRepositoryMock.Verify(repo => repo.DeleteAsync(courseId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCourseDoesNotExist()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            _courseRepositoryMock
                .Setup(repo => repo.DeleteAsync(courseId))
                .ThrowsAsync(new Exception("Course not found"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new DeleteCourseCommand(courseId), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Course not found");
            _courseRepositoryMock.Verify(repo => repo.DeleteAsync(courseId), Times.Once);
        }
    }
}
