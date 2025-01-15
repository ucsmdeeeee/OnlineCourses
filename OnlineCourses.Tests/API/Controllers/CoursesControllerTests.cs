using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineCourses.Application.Commands;
using OnlineCourses.Application.DTOs.Courses;
using OnlineCourses.Application.Queries;
using Xunit;

namespace OnlineCourses.Tests.API.Controllers
{
    public class CoursesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CoursesController _controller;

        public CoursesControllerTests()
        {
            // 1. Мок объекта IMediator
            _mediatorMock = new Mock<IMediator>();

            // 2. Создаём фиктивного пользователя с ролью "Teacher"
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Teacher")
            }, "mockAuth"));

            // 3. Инициализируем контроллер и встраиваем мок IMediator
            _controller = new CoursesController(_mediatorMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfCourses()
        {
            // Arrange
            var expectedCourses = new List<GetAllCoursesDto>
            {
                new GetAllCoursesDto { Id = Guid.NewGuid(), Title = "Course 1", Description = "Desc 1" },
                new GetAllCoursesDto { Id = Guid.NewGuid(), Title = "Course 2", Description = "Desc 2" },
            };

            // Настраиваем мок: при вызове Send(GetCoursesQuery) вернуть список курсов
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCoursesQuery>(), default))
                .ReturnsAsync(expectedCourses);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedCourses);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenCourseFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var expectedCourse = new GetCourseByIdDto
            {
                Id = courseId,
                Title = "Some Course",
                Description = "Some Description",
                Price = 100m
            };

            // Настраиваем мок: при вызове Send(GetCourseByIdQuery(courseId)) вернуть найденный курс
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == courseId), default))
                .ReturnsAsync(expectedCourse);

            // Act
            var result = await _controller.GetById(courseId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(expectedCourse);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCourseNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            // Настраиваем мок: при вызове Send(GetCourseByIdQuery(courseId)) вернуть null
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == courseId), default))
                .ReturnsAsync((GetCourseByIdDto)null);

            // Act
            var result = await _controller.GetById(courseId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenCourseCreated()
        {
            // Arrange
            var userId = Guid.Parse(_controller.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var dto = new CreateCourseDto
            {
                Title = "New Course",
                Description = "New Description",
                Price = 50m
            };

            var createdCourseId = Guid.NewGuid();

            // Настраиваем мок: при вызове Send(CreateCourseCommand(...)) вернуть созданный Id
            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<CreateCourseCommand>(cmd =>
                        cmd.Title == dto.Title &&
                        cmd.Description == dto.Description &&
                        cmd.Price == dto.Price &&
                        cmd.AuthorId == userId),
                    default))
                .ReturnsAsync(createdCourseId);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult!.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues!["id"].Should().Be(createdCourseId);
            createdResult.Value.Should().Be(createdCourseId);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenCourseUpdated()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var dto = new UpdateCourseDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Price = 70m
            };

            // Настраиваем мок: команда на обновление ничего не возвращает (или возвращает Unit)
            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<UpdateCourseCommand>(cmd =>
                        cmd.CourseId == courseId &&
                        cmd.Title == dto.Title &&
                        cmd.Description == dto.Description &&
                        cmd.Price == dto.Price),
                    default))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.Update(courseId, dto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenCourseDeleted()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            // Настраиваем мок: команда на удаление возвращает Unit (или просто Task)
            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<DeleteCourseCommand>(cmd => cmd.CourseId == courseId),
                    default))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.Delete(courseId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
