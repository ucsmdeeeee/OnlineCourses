using MediatR;
using OnlineCourses.Application.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Unit>
{
    private readonly ICourseRepository _repository;

    public UpdateCourseHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(command.CourseId);
        if (course == null)
        {
            throw new Exception($"Course with Id {command.CourseId} does not exist.");

        }

        course.Update(command.Title, command.Description, command.Price);
        await _repository.UpdateAsync(course);

        return Unit.Value; // Убедитесь, что возвращается Unit.Value
    }
}
