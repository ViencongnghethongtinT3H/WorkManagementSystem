namespace WorkManagementSystem.Features.WorkItem.UpdateNoteWorkItem;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultModel<bool>> UpdateNoteWorkItem(Request r)
    {
        var workItemRepository = _unitOfWork.GetRepository<Entities.WorkItem>();
        var workItem = await workItemRepository.GetAsync(r.WorkItemId);
        var implemenRepo = _unitOfWork.GetRepository<Implementer>();
        var imple = new Implementer
        {
            IssuesId = r.WorkItemId,
            IsTaskItem = false,
            UserReceiveId = r.UserReceiveId,
            Note = r.Note,
            DepartmentReceiveId = r.DepartmentReceiveId    
        };
        await implemenRepo.AddAsync(imple);
        _unitOfWork.Commit();

        var name = await new GetUserNameCommand
        {
            UserId = r.UserReceiveId,
        }.ExecuteAsync();

        var lstcmd = new List<NotificationCommandbase>();
        var notification = new NotificationCommandbase
        {
            Content = $"Tài khoản {name} đã cập nhật ký kiến về văn bản đến số {workItem.WorkItemNumber}",
            UserReceive = new Guid(workItem.UserIdCreated),
            UserSend = r.UserReceiveId,
            Url = r.WorkItemId.ToString(),
            NotificationType = NotificationType.WorkItem,
            NotificationWorkItemType = NotificationWorkItemType.UpdateWorkItem
        };
        lstcmd.Add(notification);
        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();

        await new HistoryCommand
        {
            UserId = r.UserReceiveId,
            IssueId = r.WorkItemId,
            ActionContent = $"Tài khoản {name} đã cập nhật ký kiến về văn bản đến số {workItem.WorkItemNumber}"
        }.ExecuteAsync();
        return ResultModel<bool>.Create(true);
    }
}
