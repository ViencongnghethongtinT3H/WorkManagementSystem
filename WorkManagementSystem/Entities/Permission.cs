using System.ComponentModel.DataAnnotations;

namespace WorkManagementSystem.Entities;

public class Permission : EntityBase
{
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
}
