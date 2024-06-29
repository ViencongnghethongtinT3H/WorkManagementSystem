namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch;

public class Request
{
    public Guid WorkflowId { get; set; }
    public List<UserProccess> UserProccess { get; set; }


}
public class UserProccess
{
    public Guid UserIds { get; set; }
    public UserWorkflowType UserWorkflowType { get; set; }
    public string? Note { get; set; }
}

