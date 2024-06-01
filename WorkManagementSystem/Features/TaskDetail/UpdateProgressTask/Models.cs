namespace WorkManagementSystem.Features.TaskDetail.UpdateProgressTask;

public class Request
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public int ProgressValue { get; set; }
    public string Note { get; set; }
}
