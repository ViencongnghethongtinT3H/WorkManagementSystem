namespace WorkManagementSystem.Features.WorkItem;

public class ImplementerModel
{
    public Guid IssuesId { get; set; }   // Id của văn bản hoặc của nhiệm vụ
    public Guid UserId { get; set; }  // Id của nhân viên thực hiện
    public Guid? DepartmentReceiveId { get; set; }  //  cơ quan nhận công văn, với trường hợp đây là công văn
    [MaxLength(100)]
    public string? Note { get; set; }  // ý kiến xử lý
}
