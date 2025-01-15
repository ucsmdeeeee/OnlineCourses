using MediatR;
using OnlineCourses.Application.DTOs.Courses;

public record GetCoursesQuery : IRequest<IEnumerable<GetAllCoursesDto>>;
