namespace WorkManagementSystem.Features.Department
{
    public class DepartmentModel
    {
        public Guid Id { get; set; }   // Id của cấp cha 
        public Guid? ParentId { get; set; }   // Id của cấp cha 
        [MaxLength(100)]
        public string Name { get; set; }  // Tên phòng ban, Tên đơn vị
        public OrganizationLevelEnum OrganizationLevel { get; set; }  // cấp độ : cấp bộ, câp tỉnh
    }
}
