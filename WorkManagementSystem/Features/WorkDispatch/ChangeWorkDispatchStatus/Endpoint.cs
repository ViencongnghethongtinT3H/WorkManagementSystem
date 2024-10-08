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
            UserId = r.UserIds.FirstOrDefault(),
        }.ExecuteAsync();
        // láy ra subject cua cong van
        var notationWorkDispatch = await new GetNotationWorkDispatchCommand
        {
            WorkDispatchId = r.WorkFlowId
        }.ExecuteAsync();
        var lstcmd = new List<NotificationCommandbase>();
        // notifine
        lstcmd.Add(new NotificationCommandbase
        {
            Content = $"Tài khoản {name} {r.ActionType.GetDescription()} {notationWorkDispatch} vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
            UserReceive = new Guid(userCompileId),
            UserSend = r.UserIds.FirstOrDefault(),
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
            UserId = r.UserIds.FirstOrDefault(),
            IssueId = r.WorkFlowId ,
            ActionContent = $"Tài khoản {name} {r.ActionType.GetDescription()} {notationWorkDispatch}"
        }.ExecuteAsync();

        await new NoteCommand
        {
            UserId = r.UserId,
            WorkFlow = r.WorkFlowId,
            Notes = r.Note
        }.ExecuteAsync();

        await SendAsync(result);
    }
}
