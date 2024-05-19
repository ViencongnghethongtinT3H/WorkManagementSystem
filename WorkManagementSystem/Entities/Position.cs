using System.ComponentModel.DataAnnotations;

namespace WorkManagementSystem.Entities
{
    public class Position : EntityBase
    {
        [MaxLength(100)]
        [Required]
        public string PositionName { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
