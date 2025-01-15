using MediatR;

namespace OnlineCourses.Application.Commands;

public record DeleteCourseCommand(Guid CourseId) : IRequest<Unit>;

