public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course> GetByIdAsync(Guid id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Guid id);
}
