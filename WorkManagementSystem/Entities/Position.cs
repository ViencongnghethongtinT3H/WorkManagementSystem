namespace WorkManagementSystem.Entities;

public class Position : EntityBase
{
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    
}
