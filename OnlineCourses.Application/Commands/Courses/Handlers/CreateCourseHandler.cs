using MediatR;
using OnlineCourses.Application.Commands;
using OnlineCourses.Domain.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Guid>
{
    private readonly ICourseRepository _repository;

    public CreateCourseHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            Price = command.Price,
            AuthorId = command.AuthorId
        };

        await _repository.AddAsync(course);
        return course.Id;

    }
}
