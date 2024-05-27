namespace WorkManagementSystem.Features.Notification;

public class Data
{

    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> UpdateStatusReadNotification(Request r)
    {
        var notificationRepository = _unitOfWork.GetRepository<Entities.Notification>();
        var notification = await notificationRepository.GetAll().FirstOrDefaultAsync(x => x.Id == r.NotificationId);

        if (notification is not null)
        {
            notification.IsRead = true;
            notification.ReceivingTime = DateTime.Now;
            notification.Updated = DateTime.Now;
            notificationRepository.Update(notification);
            await _unitOfWork.CommitAsync();
            return true;
        }
        return false;
    }
}
