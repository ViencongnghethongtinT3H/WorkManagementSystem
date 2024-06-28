namespace WorkManagementSystem.Features
{
    public class RequestImplementer
    {
        public List<ImplementerDto> Implementers { get; set; }
    }
    public class ImplementerDto
    {
        public Guid? DepartmentReceiveId { get; set; }  //  cơ quan nhận công văn, với trường hợp đây là công văn
        [MaxLength(100)]
        public string? Note { get; set; }  // ý kiến xử lý
        public Guid UserReceiveId { get; set; }  // Id của nhân viên thực hiện
    }
}
