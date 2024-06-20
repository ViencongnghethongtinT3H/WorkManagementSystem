namespace WorkManagementSystem.Entities;

public class WorkflowStep : EntityBase
{
    public StepEnum Step { get; set; }
    public Guid UserConfirm { get; set; } // người duyệt
    public string? Note { get; set; }
    public string? FormInput { get; set; }   // 1 thông tin các trường đơn giản của form, dạng key-value // cái này có thể dùng cho Văn Bản đi
}
