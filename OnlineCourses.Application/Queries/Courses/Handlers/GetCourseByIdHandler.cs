using MediatR;
using OnlineCourses.Application.DTOs.Courses;

public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, GetCourseByIdDto>
{
    private readonly ICourseRepository _repository;

    public GetCourseByIdHandler(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetCourseByIdDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.CourseId);
        if (course == null) return null;

        return new GetCourseByIdDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            AuthorId = course.AuthorId
        };
    }
}
