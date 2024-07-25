namespace WorkManagementSystem.Features.WorkDispatch.PublishWorkDispatch;

public class Endpoint : Endpoint<Request, ResultModel<Response>, Mapper>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkDispatch/publish-work-dispatch");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var lstcmd = new List<NotificationCommandbase>();
        var data = new Data(_unitOfWork);
        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = await data.CreateWorkDispatch(Map.ToEntity(r), r)
        });
        // Xử lý notification
        var receiveName = await new GetUserNameCommand{UserId = r.LeadershipDirectId}.ExecuteAsync();

        // láy ra subject cua cong van
        var notationWorkDispatch = await new GetNotationWorkDispatchCommand{WorkDispatchId = new Guid(result.Data.WorkItemId)}.ExecuteAsync();

        lstcmd.Add(new NotificationCommandbase
        {
            Content = $"Tài khoản {receiveName} đã phát hành công văn {notationWorkDispatch} vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
            UserReceive = r.LeadershipDirectId,
            UserSend = r.UserCompile,
            Url = result.Data.WorkItemId,
            NotificationType = NotificationType.WorkItem,
            NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
        });


        // Thêm phần lịch sử
        await new HistoryCommand
        {
            UserId = r.UserCompile,
            IssueId = new Guid(result.Data.WorkItemId),
            ActionContent = $"Tài khoản {receiveName} đã phát hành công văn {notationWorkDispatch}"
        }.ExecuteAsync();

        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể phát hành công văn");
        await SendAsync(result);

    }
}
