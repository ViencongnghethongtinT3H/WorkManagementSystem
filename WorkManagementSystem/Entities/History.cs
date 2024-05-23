namespace WorkManagementSystem.Entities;

public class History : EntityBase
{
    public DateTime ActionTime { get; set; } = DateTime.Now;
    public Guid UserId { get; set; }
    public Guid IssueId { get; set; }
    [MaxLength(256)]
    public string actionContent { get; set; }
}
