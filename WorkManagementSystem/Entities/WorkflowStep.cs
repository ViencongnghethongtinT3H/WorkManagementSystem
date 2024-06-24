namespace WorkManagementSystem.Entities;

// lưu các bước của văn bản đến
public class WorkflowStep : EntityBase
{
    public Guid WorkflowId { get; set; }
    public StepEnum Step { get; set; }
    public Guid UserConfirm { get; set; } // người xử lý của bước đấy
    public string? Note { get; set; }
    
}
