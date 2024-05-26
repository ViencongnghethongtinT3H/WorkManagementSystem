namespace WorkManagementSystem.Features.TaskDetail;

public class TaskDetailModel
{
    public Guid WorkItemId { get; set; }    // Link tới luồng công văn 
    //public Guid DepartmentSentId { get; set; }    // Đơn vị giao nhiệm vụ
    public Guid DepartmentReceiveId { get; set; }    // Đơn vị nhận nhiệm vụ
    public string Content { get; set; }   // nội dung công việc    
    [MaxLength(1000)]
    public string? KeyWord { get; set; }   // từ khoá 
    public DateTime? Dealine { get; set; }  // Thời hạn xử lý       
    //public ProcessingStatusEnum ProcessingStatus { get; set; }  // trạng thái của nhiệm vụ
    public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
    public bool IsPeriodical { get; set; }  // Có định kỳ hay ko
    public PeriodicalEnum Periodical { get; set; }  // định kỳ theo tháng, quý, năm
    public DateTime? DealinePeriodical { get; set; }  // thời gian kết thúc xử lý định kỳ
    public Guid UserCreateTaskId { get; set; }    // Người nhập văn bản
    public Guid LeadershipDirectId { get; set; }   // Lãnh đạo chỉ đạo
}
