using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Application.Commands;
using OnlineCourses.Application.Queries;
using MediatR;
using OnlineCourses.Application.DTOs.Users;

namespace OnlineCourses.Tests.API;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnCreatedResult_WhenSuccessful()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Role = "Student",
            Password = "password123"
        };

        var command = new RegisterUserCommand(dto.Name, dto.Email, dto.Role, dto.Password);
        var expectedUserId = Guid.NewGuid();

        // Настройка мока MediatR: команда возвращает Guid нового пользователя
        _mediatorMock
            .Setup(m => m.Send(It.Is<RegisterUserCommand>(cmd =>
                cmd.Name == dto.Name &&
                cmd.Email == dto.Email &&
                cmd.Role == dto.Role &&
                cmd.Password == dto.Password), default))
            .ReturnsAsync(expectedUserId);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().Be(expectedUserId);
    }

    [Fact]
    public async Task Login_ShouldReturnOkResult_WithToken_WhenSuccessful()
    {
        // Arrange
        var dto = new LoginUserDto
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        var command = new LoginUserCommand(dto.Email, dto.Password);
        var expectedToken = "fake-jwt-token";

        // Настройка мока MediatR: команда возвращает токен
        _mediatorMock
            .Setup(m => m.Send(It.Is<LoginUserCommand>(cmd =>
                cmd.Email == dto.Email &&
                cmd.Password == dto.Password), default))
            .ReturnsAsync(expectedToken);

        // Act
        var result = await _controller.Login(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new { Token = expectedToken });
    }


    [Fact]
    public async Task GetById_ShouldReturnOkResult_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new GetUserByIdDto
        {
            Id = userId,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Role = null
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetById(userId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        // Сравниваем свойства объектов
        okResult!.Value.Should().BeEquivalentTo(expectedUser);
    }


    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default)).ReturnsAsync((GetUserByIdDto)null);

        // Act
        var result = await _controller.GetById(userId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
