using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Infrastructure.Persistence;
using OnlineCourses.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Infrastructure.Repositories
{
    public class CourseRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public CourseRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddCourseToDatabase()
        {
            using var context = new AppDbContext(_dbOptions);
            var repository = new CourseRepository(context);

            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Test Course",
                Description = "Test Description",
                Price = 100m
            };

            await repository.AddAsync(course);

            var retrievedCourse = await context.Courses.FindAsync(course.Id);
            retrievedCourse.Should().NotBeNull();
            retrievedCourse.Title.Should().Be("Test Course");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCourse_WhenCourseExists()
        {
            using var context = new AppDbContext(_dbOptions);
            var repository = new CourseRepository(context);

            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Test Course",
                Description = "Test Description",
                Price = 100m
            };

            await context.Courses.AddAsync(course);
            await context.SaveChangesAsync();

            var retrievedCourse = await repository.GetByIdAsync(course.Id);

            retrievedCourse.Should().NotBeNull();
            retrievedCourse.Title.Should().Be("Test Course");
        }
    }
}
