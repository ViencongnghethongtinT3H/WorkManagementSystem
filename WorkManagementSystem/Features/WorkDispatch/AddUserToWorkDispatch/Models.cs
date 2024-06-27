namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch;

public class Request
{
    public List<Guid> UserIds { get; set; }
    public Guid WorkflowId { get; set; }
    public UserWorkflowType UserWorkflowType { get; set; }
}

