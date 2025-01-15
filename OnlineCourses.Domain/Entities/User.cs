using System.Text.RegularExpressions;

namespace OnlineCourses.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Инициализация по умолчанию
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public User() { }

        public User(string name, string email, string role, string passwordHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Role = role;
            PasswordHash = passwordHash;
        }

        public void Update(string name, string role)
        {
            Name = name;
            Role = role;
        }

        public static void ValidateEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^\S+@\S+\.\S+$"))
            {
                throw new FormatException("Invalid email format");
            }
        }

    }
}