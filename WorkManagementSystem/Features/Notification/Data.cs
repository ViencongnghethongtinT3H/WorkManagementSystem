namespace WorkManagementSystem.Features.Notification;

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
        var query = await notificationRepository.FindBy(x => x.UserReceive == r.UserId && x.IsRead == false)
            .OrderByDescending(x => x.Created).ToListAsync();
        return new Response
        {
            TotalWorkItem = query.Count,
            TotalWorkItemNotification = query.Count(x => x.NotificationType == NotificationType.WorkItem),
            TotalTaskNotification = query.Count(x => x.NotificationType == NotificationType.Task),
            NotificationModels = query.Select(x => new NotificationModel
            {
                Content = x.Content,
                SendingTime = x.SendingTime.ToFormatString("dd/mm/yyyy"),
                Url = x.Url,
            }).ToList()
        };
    }
}
