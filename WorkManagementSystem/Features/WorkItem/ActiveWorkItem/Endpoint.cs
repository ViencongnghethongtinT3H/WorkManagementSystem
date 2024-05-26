namespace WorkManagementSystem.Features.WorkItem.ActiveWorkItem;

public class Endpoint : Endpoint<Request, ResultModel<Response>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkItem/active-workItem");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userNotifications = new List<Guid>();
        var data = new Data(_unitOfWork);
        string? workItemId;
        (workItemId, userNotifications) = await data.ActiveWorkItem(r);
        var lstcmd = new List<NotificationCommandbase>();

        var name = await new GetUserNameCommand
        {
            UserId = r.UserCreatedId,
        }.ExecuteAsync();

        foreach (var notification in userNotifications)
        {
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} chuyển một công văn tới mục Văn Bản đến của bạn",
                UserReceive = notification,
                UserSend = r.UserCreatedId,
                Url = "",
                NotificationType = NotificationType.WorkItem,
                NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
            });
        }
        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();
       
        await new HistoryCommand
        {
            UserId = r.UserCreatedId,
            IssueId = r.WorkItemId,
            ActionContent = $"Tài khoản {name} chuyển một công văn tới mục Văn Bản đến của bạn",
            
        }.ExecuteAsync();


        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = workItemId,
        });

        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể thêm công văn");

        await SendAsync(result);

    }
}
