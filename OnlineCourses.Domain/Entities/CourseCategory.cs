namespace OnlineCourses.Domain.Entities;

public class CourseCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private CourseCategory() { }

    public CourseCategory(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
