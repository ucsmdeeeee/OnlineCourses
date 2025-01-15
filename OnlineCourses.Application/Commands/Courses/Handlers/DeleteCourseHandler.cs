using MediatR;
using OnlineCourses.Application.Commands;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Unit>
{
    private readonly ICourseRepository _repository;

    public DeleteCourseHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.CourseId);
        return Unit.Value; // Убедитесь, что возвращается Unit.Value
    }
}
