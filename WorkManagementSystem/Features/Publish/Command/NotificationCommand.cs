namespace WorkManagementSystem.Features.Publish.Command;

public class LstNotificationCommand : ICommand<bool>
{
    public List<NotificationCommandbase> NotificationCommands { get; set; }
}


public class NotificationCommand : NotificationCommandbase,  ICommand<bool>
{

}
public class NotificationCommandbase
{
    public Guid UserSend { get; set; }
    public Guid UserReceive { get; set; }
    public DateTime? ReceivingTime { get; set; }
    public string Content { get; set; }
    public string? Url { get; set; }
    public NotificationType NotificationType { get; set; }
    public NotificationWorkItemType NotificationWorkItemType { get; set; }
    public bool IsRead { get; set; } = false;
}
