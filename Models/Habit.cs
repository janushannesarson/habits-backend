namespace habitsbackend.Models;

public class Habit
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IList<Entry>? Entries { get; set; }
    public string UserId { get; set; }

}
