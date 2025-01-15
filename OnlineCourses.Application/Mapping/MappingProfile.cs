using AutoMapper;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Application.DTOs.Courses;
using OnlineCourses.Application.DTOs.Users;

namespace OnlineCourses.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping для User
            CreateMap<User, GetUserByIdDto>();
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Пароль хэшируется в обработчике

            // Mapping для Course
            CreateMap<Course, GetCourseByIdDto>();
            CreateMap<Course, GetAllCoursesDto>();
            CreateMap<CreateCourseDto, Course>()
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore()); // AuthorId задается через обработчик
            CreateMap<UpdateCourseDto, Course>();
        }
    }
}
