using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Application.DTOs.Users;
using OnlineCourses.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Регистрирует нового пользователя.
    /// </summary>
    /// <param name="command">Данные пользователя.</param>
    /// <returns>Id созданного пользователя.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var command = new RegisterUserCommand(dto.Name, dto.Email, dto.Role, dto.Password);
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        try
        {
            var command = new LoginUserCommand(dto.Email, dto.Password);
            var token = await _mediator.Send(command);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound();

        var result = new GetUserByIdDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };

        return Ok(result);
    }
}
