namespace OnlineCourses.Application.DTOs.Courses
{
    public class GetAllCoursesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid AuthorId { get; set; }
    }

}
