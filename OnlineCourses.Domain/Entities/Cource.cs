public class Course
{
    private decimal _price;

    public Guid Id { get;  set; }
    public string Title { get; set; } = string.Empty; // Значение по умолчанию
    public string Description { get; set; } = string.Empty; // Значение по умолчанию
    public decimal Price {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price cannot be negative.");
            _price = value;
        }
    }
    public Guid AuthorId { get;  set; }

    public Course() { }

    public Course(string title, string description, decimal price, Guid authorId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Price = price;
        AuthorId = authorId;
    }

    public void Update(string title, string description, decimal price)
    {
        Title = title;
        Description = description;
        Price = price;
    }
}
