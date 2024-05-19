namespace WorkManagementSystem.Entities;

public class UserPermission : EntityBase
{
    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }
}
