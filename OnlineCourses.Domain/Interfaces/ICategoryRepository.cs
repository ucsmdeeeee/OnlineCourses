using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<CourseCategory>> GetAllAsync();
    Task<CourseCategory> GetByIdAsync(Guid id);
    Task AddAsync(CourseCategory category);
    Task UpdateAsync(CourseCategory category);
    Task DeleteAsync(Guid id);
}
