using MediatR;
using OnlineCourses.Application.DTOs.Courses;

public class GetCoursesHandler : IRequestHandler<GetCoursesQuery, IEnumerable<GetAllCoursesDto>>
{
    private readonly ICourseRepository _courseRepository;

    public GetCoursesHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<GetAllCoursesDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllAsync();

        return courses.Select(course => new GetAllCoursesDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description
        });
    }
}
