using AutoMapper;
using OnlineCourses.Application.Mapping;

namespace OnlineCourses.Tests.Helpers
{
    public static class AutoMapperHelper
    {
        public static IMapper Mapper
        {
            get
            {
                var config = new MapperConfiguration(cfg =>
                {
                    // Подключение всех профилей из Application
                    cfg.AddProfile<MappingProfile>();
                });

                return config.CreateMapper();
            }
        }
    }
}
