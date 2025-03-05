using Microsoft.EntityFrameworkCore;
using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Contexts;

public class UniversityContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public UniversityContext(DbContextOptions<UniversityContext> options)
    : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
        .HasOne(e => e.Department)
        .WithMany(d => d.Employees)
        .HasForeignKey(e => e.DepartmentId)
        .OnDelete(DeleteBehavior.SetNull); // Prevent cascading delete

        base.OnModelCreating(modelBuilder);
    }
}
