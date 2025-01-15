using FluentAssertions;
using Moq;
using OnlineCourses.Application.DTOs.Courses;
using OnlineCourses.Application.Queries;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCourses.Tests.Application.Courses.Queries.Handlers
{
    public class GetCoursesHandlerTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly GetCoursesHandler _handler;

        public GetCoursesHandlerTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _handler = new GetCoursesHandler(_courseRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfCourses_WhenCoursesExist()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = Guid.NewGuid(), Title = "Course 1", Description = "Description 1", Price = 100m },
                new Course { Id = Guid.NewGuid(), Title = "Course 2", Description = "Description 2", Price = 200m }
            };

            var expectedDtos = courses.Select(c => new GetAllCoursesDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description
            }).ToList();

            _courseRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(courses);

            // Act
            var result = await _handler.Handle(new GetCoursesQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCoursesExist()
        {
            // Arrange
            _courseRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Course>());

            // Act
            var result = await _handler.Handle(new GetCoursesQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
