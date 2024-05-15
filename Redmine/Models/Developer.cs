using Redmine.Models;

public class Developer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<DeveloperTask> DeveloperTasks { get; set; } = new List<DeveloperTask>();
}