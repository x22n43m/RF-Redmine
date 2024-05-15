using Redmine.Models;

public class Task
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public int ManagerId { get; set; } 
    public DateTime Deadline { get; set; }
    public Project Project { get; set; }
    public Manager Manager { get; set; }
    public ICollection<DeveloperTask> DeveloperTasks { get; set; } = new List<DeveloperTask>();
}
