namespace WorkManagementSystem.Entities
{
    // Đơn vị nhận công văn
    public class ReceiveCompany : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public Guid? AccountReceiveId { get; set; }   // Id của văn thư đơn vị nhận
    }
}
