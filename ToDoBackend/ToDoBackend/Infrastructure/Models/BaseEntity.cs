namespace ToDoBackend;


public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the universally unique identifier (UID) of the entity.
    /// </summary>
    public Guid Uid { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the username of the creator. (Required)
    /// </summary>
    public string CreatedBy { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timestamp when the entity was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the username of the last modifier.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the entity is marked as deleted.
    /// </summary>
    public bool Deleted { get; set; }
}
