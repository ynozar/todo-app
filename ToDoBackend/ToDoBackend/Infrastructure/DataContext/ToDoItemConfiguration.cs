using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDoBackend.DataContext;

public class ToDoItemConfiguration: BaseEntityConfiguration<ToDoItem>
{
    public override void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.IsComplete).IsRequired().HasDefaultValue(false);


        builder.HasOne<Group>(i => i.Group)
            .WithMany(i => i.ToDoItems)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(i => i.GroupId);
        builder.Navigation(i=> i.Group).AutoInclude();

    }
}