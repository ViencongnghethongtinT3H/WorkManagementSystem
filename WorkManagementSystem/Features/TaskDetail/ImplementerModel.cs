namespace WorkManagementSystem.Features.TaskDetail
{
    public class ImplementerModel
    {
        public Guid UserReceiveId { get; set; }  // Id của nhân viên thực hiện
        [MaxLength(100)]
        public string? Note { get; set; }  // ý kiến xử lý
    }
}
