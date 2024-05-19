namespace WorkManagementSystem.Entities;

public class Role : EntityBase
{
    [MaxLength(100)]
    public string Name { get; set; }

}
