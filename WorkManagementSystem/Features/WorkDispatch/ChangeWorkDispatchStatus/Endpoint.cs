using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;

public class Endpoint : Endpoint<Request, ResultModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;   
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/workDispatch/change-work-dispatch-status");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.ChangeApproveWorkDispatch(r);
        var lstcmd = new List<NotificationCommandbase>();
        var name = await data.GetUserName(r);
        //var receiveName = await new GetUserNameCommand
        //{
        //    UserId = r.RequestImplementer.Implementers.FirstOrDefault().UserReceiveId,
        //}.ExecuteAsync();

        await new HistoryCommand
        {
            UserId = r.UserId,
            IssueId = r.WorkFlowId ,
            ActionContent = $"Tài khoản {name} đã thay đổi trạng thái văn bản"
        }.ExecuteAsync();

        //Todo: tạm thời phần notification thay đổi trạng thái để lại 

        //foreach (var notification in userNotifications)
        //{
        //    lstcmd.Add(new NotificationCommandbase
        //    {
        //        Content = $"Tài khoản {name} chuyển một công văn tới mục Văn Bản đến của {receiveName}",
        //        UserReceive = notification,
        //        UserSend = r.UserId,
        //        Url = r.WorkFlowId.ToString(),
        //        NotificationType = NotificationType.WorkItem,
        //        NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
        //    });
        //}
        //await new LstNotificationCommand
        //{
        //    NotificationCommands = lstcmd
        //}.ExecuteAsync();

        await SendAsync(result);
    }
}
