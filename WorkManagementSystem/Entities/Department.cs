using System.ComponentModel.DataAnnotations;
using WorkManagementSystem.Entities.Enums;

namespace WorkManagementSystem.Entities
{
    // Cấp phòng ban
    public class Department: EntityBase   
    {
        public Guid ParentId { get; set; }   // Id của cấp cha 
        [MaxLength(100)]
        public string Name { get; set; }  // Tên phòng ban, Tên đơn vị
        public OrganizationLevelEnum OrganizationLevel { get; set; }  // cấp độ : cấp bộ, câp tỉnh

        
    }
}
