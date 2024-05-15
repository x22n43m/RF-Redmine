using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Redmine;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }

    [ForeignKey("ProjectType")]
    [Column("type_id")]
    public int TypeId { get; set; }  

    public ProjectType ProjectType { get; set; }
    public string Description { get; set; }

    public List<Task> Tasks { get; set; }

    public List<ProjectDeveloper> ProjectDevelopers { get; set; }
}