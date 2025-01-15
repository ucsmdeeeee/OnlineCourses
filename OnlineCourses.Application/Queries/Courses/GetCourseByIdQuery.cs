using MediatR;
using OnlineCourses.Application.DTOs.Courses;

public record GetCourseByIdQuery(Guid CourseId) : IRequest<GetCourseByIdDto>;
