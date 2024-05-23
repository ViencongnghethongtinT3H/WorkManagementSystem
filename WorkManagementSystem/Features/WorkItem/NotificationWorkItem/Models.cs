namespace WorkManagementSystem.Features.WorkItem.NotificationWorkItem
{
    public class Request
    {
        public Guid UserId { get; set; }
    }
    public class Response
    {
        public int TotalWorkItem { get; set; }
        public string? Message { get; set; }
    }
}
