using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDoBackend.DataContext;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasAlternateKey(i => i.Uid);

        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(i => i.Uid).IsRequired().HasDefaultValueSql("uuid_generate_v4()").ValueGeneratedOnAdd();
        builder.Property(i => i.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(i => i.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(i => i.ModifiedBy).HasMaxLength(256);
        builder.Property(i => i.Deleted).IsRequired().HasDefaultValue(false);
    }
}
