namespace Redmine.Models
{
    public class DeveloperTask
    {
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
