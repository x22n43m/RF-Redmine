namespace Redmine.Classes
{
    public class TaskDto2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int ProjectId { get; set; }
    }
}
