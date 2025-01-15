using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OnlineCourses.Application.Commands;
using OnlineCourses.Application.DTOs.Courses;
using OnlineCourses.Application.Queries;
using OnlineCourses.Domain.Entities;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _mediator.Send(new GetCoursesQuery());
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(id)); // Использование MediatR
        if (course == null) return NotFound();
        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var command = new CreateCourseCommand(dto.Title, dto.Description, dto.Price, userId);
        var courseId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = courseId }, courseId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var command = new UpdateCourseCommand(id, dto.Title, dto.Description, dto.Price);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var command = new DeleteCourseCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
