namespace WorkManagementSystem.Features.History;

public class Request
{
    public Guid IssuesId { get; set; }
}

public class Response
{
    public DateTime ActionTime { get; set; } = DateTime.Now;
    public string UserUpdated { get; set; }
    public string ActionContent { get; set; }
}
