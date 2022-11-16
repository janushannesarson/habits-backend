namespace habitsbackend.Models;

public class Habit
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string UserId { get; set; }

    public Habit(int? id, string name, string? description, string userId)
    {
        Id = id;
        Name = name;
        Description = description;
        UserId = userId;
    }
}