using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using P01_StudentSystem.Data.Models.Configuration;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
  

    public StudentSystemContext()
    {

    }

    public StudentSystemContext(DbContextOptions<StudentSystemContext> options) : base(options) 
    {
    
    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet <Homework> Homeworks { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCourse> StudentsCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

            optionsBuilder.UseSqlServer("Server=./SQLEXPRESS;Database=SoftUni;Integrated Security=True; Encrypt= False;");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ResourceConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new HomeworkConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new StudentCourseConfiguration());
    }
}