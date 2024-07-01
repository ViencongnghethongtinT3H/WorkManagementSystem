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
        var (result,userCompileId) = await data.ChangeApproveWorkDispatch(r);

        // Xử lý notification
        var name = await  new GetUserNameCommand
        {
            UserId = r.UserId
        }.ExecuteAsync();
        
        var receiveName = await new GetUserNameCommand
        {
            UserId = new Guid(userCompileId)
        }.ExecuteAsync();

        // láy ra subject cua cong van
        var subjectWorkDispatch = await new GetSubjectWorkDispatchCommand
        {
            WorkDispatchId = r.WorkFlowId
        }.ExecuteAsync();
        var lstcmd = new List<NotificationCommandbase>();
        // notifine
        lstcmd.Add(new NotificationCommandbase
        {
            Content = $"Tài khoản {name} đã cập nhật lại trạng thái của công văn {subjectWorkDispatch} do {receiveName} tạo",
            UserReceive = new Guid(userCompileId),
            UserSend = r.UserId,
            Url = r.WorkFlowId.ToString(),
            NotificationType = NotificationType.WorkItem,
            NotificationWorkItemType = NotificationWorkItemType.UpdateProgressTask
        });

        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();

        // history
        await new HistoryCommand
        {
            UserId = r.UserId,
            IssueId = r.WorkFlowId ,
            ActionContent = $"Tài khoản {name} đã thay đổi trạng thái công văn"
        }.ExecuteAsync();
        await SendAsync(result);
    }
}
