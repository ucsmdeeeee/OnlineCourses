using MediatR;

namespace OnlineCourses.Application.Commands;

public record CreateCourseCommand(string Title, string Description, decimal Price, Guid AuthorId) : IRequest<Guid>;

