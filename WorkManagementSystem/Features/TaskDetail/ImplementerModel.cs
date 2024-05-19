namespace WorkManagementSystem.Features.TaskDetail
{
    public class ImplementerModel
    {
        public Guid IssuesId { get; set; }   // Id của văn bản hoặc của nhiệm vụ
        public Guid UserId { get; set; }  // Id của nhân viên thực hiện
        [MaxLength(100)]
        public string? Note { get; set; }  // ý kiến xử lý
    }
}
