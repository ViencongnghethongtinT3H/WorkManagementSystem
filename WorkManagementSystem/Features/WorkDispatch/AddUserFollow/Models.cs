namespace WorkManagementSystem.Features.WorkDispatch.AddUserFollow;

public class Request
{
    public Guid UserId { get; set; }
    public FollowUserType FollowUserType { get; set; }
}
public enum FollowUserType
{
    Follow = 1,
    Combination = 2
}
