namespace Redmine.Classes
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public DateTime Deadline { get; set; }
        public int DeveloperId { get; set; } 
    }

}
