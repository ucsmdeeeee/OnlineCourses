namespace OnlineCourses.Application.DTOs.Users
{
    public class GetUserByIdDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Укажите значение по умолчанию
        public string Email { get; set; } = string.Empty; // Укажите значение по умолчанию
        public string Role { get; set; } = string.Empty; // Укажите значение по умолчанию
    }


}
