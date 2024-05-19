namespace WorkManagementSystem.Entities;

public abstract class EntityBase
{
    public Guid Id { get; set; } 
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
    public string? UserIdCreated { get; set; }
    public string? UserIdUpdated { get; set; }
    public StatusEnum Status { get; set; }
}
