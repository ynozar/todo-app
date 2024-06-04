using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDoBackend.DataContext;

public class GroupConfiguration: BaseEntityConfiguration<Group>
{
    public override void Configure(EntityTypeBuilder<Group> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.isDefault).IsRequired().HasDefaultValue(false);
        
        builder.Navigation(i => i.ToDoItems).AutoInclude();
        
        builder.HasOne<User>(i => i.User)
            .WithMany(i => i.Groups)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(i => i.UserId);

    }
}