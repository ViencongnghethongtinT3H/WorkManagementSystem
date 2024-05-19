namespace WorkManagementSystem.Entities;

public class User : EntityBase
{
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }  // tên user
    public Guid DepartmentId { get; set; }  // Phòng ban
    public Guid PositionId { get; set; }  // Vị trí
    [MaxLength(100)]
    [Required]
    public string Email { get; set; }
    [MaxLength(20)]
    public string? Phone { get; set; }

    public string PasswordHash{ get; set; }


}
