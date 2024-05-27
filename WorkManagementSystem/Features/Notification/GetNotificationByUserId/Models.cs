namespace WorkManagementSystem.Features.Notification.GetNotificationByUserId;

public class Request
{
    public Guid UserId { get; set; }
}
public class Response
{
    public int TotalWorkItem { get; set; }
    public int TotalWorkItemNotification { get; set; }
    public int TotalTaskNotification { get; set; }
    public List<NotificationModel> NotificationModels { get; set; }
}
public class NotificationModel
{
    public Guid Id { get; set; }    
    public DateTime SendingTime { get; set; }
    public string Content { get; set; }
    public string? Url { get; set; }
    public NotificationWorkItemType NotificationWorkItemType { get; set; }
}

