namespace OnlineCourses.Domain.Entities;

public class CourseEnrollment
{
    public Guid Id { get; private set; }
    public Guid CourseId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime EnrolledAt { get; private set; }

    private CourseEnrollment() { }

    public CourseEnrollment(Guid courseId, Guid userId)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        UserId = userId;
        EnrolledAt = DateTime.UtcNow;
    }
}
