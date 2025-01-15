using MediatR;
using OnlineCourses.Domain.Interfaces;
using OnlineCourses.Application.DTOs;
using AutoMapper;
using OnlineCourses.Application.DTOs.Users;

namespace OnlineCourses.Application.Queries;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserByIdDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.Id);

        if (user == null)
        {
            return null; // или выбросьте исключение, если требуется
        }

        return new GetUserByIdDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
}

