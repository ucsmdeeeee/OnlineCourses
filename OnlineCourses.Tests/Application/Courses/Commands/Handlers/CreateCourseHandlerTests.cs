using Xunit;
using Moq;
using Moq.Language.Flow;
using FluentAssertions;
using OnlineCourses.Application.Commands;

namespace OnlineCourses.Tests.Application.Courses.Commands.Handlers
{
    public class CreateCourseHandlerTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly CreateCourseHandler _handler;

        public CreateCourseHandlerTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _handler = new CreateCourseHandler(_courseRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateCourse_WhenValidRequest()
        {
            // Arrange
            var command = new CreateCourseCommand("Test Title", "Test Description", 100m, Guid.NewGuid());

            _courseRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Course>()))
                .Returns(Task.CompletedTask); // Используем Task.CompletedTask, так как метод ничего не возвращает

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _courseRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Course>(c =>
                c.Title == command.Title &&
                c.Description == command.Description &&
                c.Price == command.Price &&
                c.AuthorId == command.AuthorId)), Times.Once);
        }


        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var command = new CreateCourseCommand("Test Title", "Test Description", 100m, Guid.NewGuid());

            _courseRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Course>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
            _courseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Course>()), Times.Once);
        }
    }
}
