using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Infrastructure.Persistence;
using BCrypt.Net;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email); // Исправление
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public bool ValidatePassword(string password, string hashedPassword)
    {
        // Используем BCrypt.Net-Next для проверки пароля
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
