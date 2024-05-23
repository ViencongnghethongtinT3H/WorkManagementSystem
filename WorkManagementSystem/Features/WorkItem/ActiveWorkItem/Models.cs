namespace WorkManagementSystem.Features.WorkItem.ActiveWorkItem;

public class Request
{
    public Guid WorkItemId { get; set; }
    public DateTime? Dealine { get; set; }  // Thời hạn xử lý
    public DateTime? EvictionTime { get; set; }  // Thời hạn thu hồi
    public Guid? UserId { get; set; }    // chủ trì công văn này 
    public List<ImplementerDto> Implementers { get; set; }  
    public Guid UserCreatedId { get; set; }
}
public class ImplementerDto
{
    public Guid? DepartmentReceiveId { get; set; }  //  cơ quan nhận công văn, với trường hợp đây là công văn
    [MaxLength(100)]
    public string? Note { get; set; }  // ý kiến xử lý
    public Guid UserReceiveId { get; set; }  // Id của nhân viên thực hiện
}

public class Response
{
    public string Message => "Đã chuyển xử lý thành công";
    public string? WorkItemId { get; set; }
}

