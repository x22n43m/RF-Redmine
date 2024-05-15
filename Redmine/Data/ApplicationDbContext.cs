using Microsoft.EntityFrameworkCore;
using Redmine;
using Redmine.Models;

public class ApplicationDbContext : DbContext
{
    // Add a constructor that takes DbContextOptions<ApplicationDbContext>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet properties
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Developer> Developers { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectType> ProjectTypes { get; set; }
    public DbSet<ProjectDeveloper> ProjectDevelopers { get; set; }
    public DbSet<DeveloperTask> DeveloperTasks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectDeveloper>()
            .HasKey(pd => new { pd.DeveloperId, pd.ProjectId });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.Property(e => e.Deadline)
                  .HasColumnType("date"); 
        });

        modelBuilder.Entity<ProjectDeveloper>()
            .HasOne(pd => pd.Project)
            .WithMany(p => p.ProjectDevelopers)
            .HasForeignKey(pd => pd.ProjectId);
        modelBuilder.Entity<ProjectType>().ToTable("project_types");

        modelBuilder.Entity<Project>()
            .Property(p => p.TypeId)
            .HasColumnName("type_id");  

        modelBuilder.Entity<Project>()
            .HasOne(p => p.ProjectType)
            .WithMany()
            .HasForeignKey(p => p.TypeId);  
        modelBuilder.Entity<Task>()
            .Property(p => p.ManagerId)
            .HasColumnName("user_id");
        modelBuilder.Entity<Task>()
            .Property(p => p.ProjectId)
            .HasColumnName("project_id");

        modelBuilder.Entity<DeveloperTask>()
            .HasKey(dt => new { dt.DeveloperId, dt.TaskId });

        modelBuilder.Entity<DeveloperTask>()
            .HasOne(dt => dt.Developer)
            .WithMany(d => d.DeveloperTasks)
            .HasForeignKey(dt => dt.DeveloperId);

        modelBuilder.Entity<DeveloperTask>()
            .HasOne(dt => dt.Task)
            .WithMany(t => t.DeveloperTasks)
            .HasForeignKey(dt => dt.TaskId);

    }

}