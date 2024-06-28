using WorkManagementSystem.Entities;
using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.AddWorkDispatch;

public class Endpoint : Endpoint<Request, ResultModel<Response>, Mapper>
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
        Post("/WorkDispatch/create");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userNotifications = new List<Guid>();
        var data = new Data(_unitOfWork, _eventImplement);
        string? workItemId;
        (workItemId, userNotifications) = await data.CreateWorkDispatch(Map.ToEntity(r), r);
        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = workItemId
        });
        var lstcmd = new List<NotificationCommandbase>();
        var name = await data.GetUserName(r);
        var receiveName = await new GetUserNameCommand
        {
            UserId = r.RequestImplementer.Implementers.FirstOrDefault().UserReceiveId,
        }.ExecuteAsync();

        await new HistoryCommand
        {
            UserId = r.UserCompile,
            IssueId = new Guid(result.Data.WorkItemId),
            ActionContent = $"Tài khoản {name} đã tạo thêm một công văn"
        }.ExecuteAsync();
        foreach (var notification in userNotifications)
        {
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} chuyển một công văn tới mục Văn Bản đến của {receiveName}",
                UserReceive = notification,
                UserSend = r.UserCompile,
                Url = workItemId,
                NotificationType = NotificationType.WorkItem,
                NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
            });
        }
        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();
        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể thêm công văn");
        await SendAsync(result);

    }
}
