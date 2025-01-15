using MediatR;
using OnlineCourses.Application.DTOs;
using OnlineCourses.Application.DTOs.Users;

namespace OnlineCourses.Application.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdDto>;
