namespace ToDoBackend.DataContext;
using Microsoft.EntityFrameworkCore;
public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("uuid-ossp");//Needed for auto generated GUIDS
        modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
/*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseModel(CompiledModel.Instance);
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=todo;User Id=ynozar;Password=onboarding;Include Error Detail=True;"); // Use Npgsql for PostgreSQL

    }
*/
}
