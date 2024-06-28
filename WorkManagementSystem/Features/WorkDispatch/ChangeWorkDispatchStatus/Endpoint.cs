using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;

public class Endpoint : Endpoint<Request, ResultModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventImplement _eventImplement;
    public Endpoint(IUnitOfWork unitOfWork, IEventImplement eventImplement)
    {
        _unitOfWork = unitOfWork;
        _eventImplement = eventImplement;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/workDispatch/change-work-dispatch-status");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        List<Guid> userNotifications = new List<Guid>();
        var data = new Data(_unitOfWork, _eventImplement);
        ResultModel<bool>? result;
        (result, userNotifications) = await data.ChangeApproveWorkDispatch(r);
        var lstcmd = new List<NotificationCommandbase>();
        var name = await data.GetUserName(r);
        var receiveName = await new GetUserNameCommand
        {
            UserId = r.RequestImplementer.Implementers.FirstOrDefault().UserReceiveId,
        }.ExecuteAsync();

        await new HistoryCommand
        {
            UserId = r.UserId,
            IssueId = r.WorkFlowId ,
            ActionContent = $"Tài khoản {name} đã tạo thêm một công văn"
        }.ExecuteAsync();
        foreach (var notification in userNotifications)
        {
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} chuyển một công văn tới mục Văn Bản đến của {receiveName}",
                UserReceive = notification,
                UserSend = r.UserId,
                Url = r.WorkFlowId.ToString(),
                NotificationType = NotificationType.WorkItem,
                NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
            });
        }
        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();

        await SendAsync(result);
    }
}
