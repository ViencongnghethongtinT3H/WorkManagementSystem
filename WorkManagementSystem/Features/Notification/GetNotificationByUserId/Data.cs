namespace WorkManagementSystem.Features.Notification.GetNotificationByUserId;

public class Data
{

    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> GetNotification(Request r)
    {
        var notificationRepository = _unitOfWork.GetRepository<Entities.Notification>();
        var query = await notificationRepository.FindBy(x => x.UserReceive == r.UserId)
            .OrderByDescending(x => x.Created).ToListAsync();
        return new Response
        {
            TotalWorkItem = query.Count,
            TotalWorkItemNotification = query.Count(x => x.NotificationType == NotificationType.WorkItem),
            TotalTaskNotification = query.Count(x => x.NotificationType == NotificationType.Task),
            NotificationModels = query.Select(x => new NotificationModel
            {
                Id = x.Id,
                Content = x.Content,
                SendingTime = x.SendingTime,
                Url = x.Url,
                IsRead = x.IsRead,
                NotificationWorkItemType = x.NotificationWorkItemType
            }).ToList()
        };
    }
}
