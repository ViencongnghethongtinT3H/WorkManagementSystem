namespace WorkManagementSystem.Features.Publish.CommandHandler;

public class NotificationCommandHandler : ICommandHandler<LstNotificationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public NotificationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(LstNotificationCommand commands, CancellationToken ct)
    {
        var notis = new List<Entities.Notification>();
        foreach (var notificationCommand in commands.NotificationCommands)
        {
            var notification = new Entities.Notification
            {
                Content = notificationCommand.Content,
                UserReceive = notificationCommand.UserReceive,
                UserSend = notificationCommand.UserSend,
                NotificationType = notificationCommand.NotificationType,
                Url = notificationCommand.Url,
                NotificationWorkItemType = notificationCommand.NotificationWorkItemType,
            };
            notis.Add(notification);
        }       
           
        var repo = _unitOfWork.GetRepository<Entities.Notification>();
        await repo.AddRangeAsync(notis);
        await _unitOfWork.CommitAsync();
        return true;
    }
}
