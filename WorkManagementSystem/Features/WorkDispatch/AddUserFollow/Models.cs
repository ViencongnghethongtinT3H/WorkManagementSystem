namespace WorkManagementSystem.Features.WorkDispatch.AddUserFollow;

public class Request
{
    public List<Guid> UserIds { get; set; }
    public Guid WorkflowId { get; set; }

    public FollowUserType FollowUserType { get; set; }
}
public enum FollowUserType
{
    Follow = 1,
    Combination = 2
}
