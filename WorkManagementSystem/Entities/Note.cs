namespace WorkManagementSystem.Entities;

public class Note : EntityBase
{
    public Guid UserId { get; set; }
    public Guid WorkFlow { get; set; }
    [MaxLength(250)]
    public string Notes { get; set; }
}
