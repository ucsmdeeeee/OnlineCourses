using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Interfaces;
using OnlineCourses.Infrastructure.Persistence;

namespace OnlineCourses.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course> GetByIdAsync(Guid id) =>
        await _context.Courses.FindAsync(id);

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses.AsNoTracking().ToListAsync();
    }


    public async Task AddAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await GetByIdAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
