using Microsoft.EntityFrameworkCore;
using WorkManagementSystem.Entities;
namespace WorkManagementSystem.Infrastructure.DbContext;

public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<FileAttach> FileAttachs { get; set; }
    public DbSet<Implementer> Implementers { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<TaskDetail> TaskDetails { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        // var connectionString = ConnectionHelper.SqlServerConn;
        var connectionString = "Server=103.229.42.125, 1433;Database=NotificationDbNew;User Id=sa;Password=Digins@2022;TrustServerCertificate=true;";
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);
        optionsBuilder.EnableSensitiveDataLogging(true);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkItem>().HasIndex(e => new { e.Status });
    }
}
