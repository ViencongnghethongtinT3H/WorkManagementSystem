namespace WorkManagementSystem.Features.TaskDetail.UpdateProgressTask;

public class Request
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Note { get; set; } = string.Empty;
    public StatusEnum Status { get; set; }  
}
