using MediatR;

namespace OnlineCourses.Application.Commands;

public record UpdateCourseCommand(Guid CourseId, string Title, string Description, decimal Price) : IRequest<Unit>;
