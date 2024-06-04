namespace ToDoBackend;

using Microsoft.EntityFrameworkCore;
//delete after removing program.cs endpoints
class ToDoDb : DbContext
{
    public ToDoDb(DbContextOptions<ToDoDb> options)
        : base(options) { }

    public DbSet<ToDoItem> Todos => Set<ToDoItem>();
}