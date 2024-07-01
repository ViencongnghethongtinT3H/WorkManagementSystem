namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch;

// Chức năng chuyển công văn cho người/n-người theo dõi (bật popup lên cho chọn người theo dõi công văn và có thêm phần vai trò:  Người theo dõi, Người phối hợp/ xử lý)
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
        Post("/workDispatch/add-user-to-work-dispatch");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        ResultModel<bool>? result = await data.AddUserToWorkDispatch(r);
        var lstcmd = new List<NotificationCommandbase>();
        var name = await new GetUserNameCommand
        {
            UserId = r.UserId
        }.ExecuteAsync(); // người thêm các người theo dõi vào công văn

        // lấy ra subject của công văn
        var subjectWorkDispatch = await new GetSubjectWorkDispatchCommand { WorkDispatchId = r.WorkflowId}.ExecuteAsync();
        foreach (var item in r.UserProccess)
        {
            var nameFlow =  await new GetUserNameCommand
            {
                UserId = r.UserId
            }.ExecuteAsync();
            // Thêm phần notification
            lstcmd.Add(new NotificationCommandbase
            {
                UserSend = r.UserId,
                Content = $"Tài khoản {nameFlow} đã được thêm vào công văn {subjectWorkDispatch} bởi {name}",
                UserReceive = item.UserIds,
                Url = r.WorkflowId.ToString(),
                NotificationType = NotificationType.WorkItem,
                NotificationWorkItemType = NotificationWorkItemType.SendTask
            });

            await new LstNotificationCommand
            {
                NotificationCommands = lstcmd
            }.ExecuteAsync();

            await new LstNotificationCommand
            {
                NotificationCommands = lstcmd
            }.ExecuteAsync();

            // thêm lịch sử
            // Thêm phần lịch sử
            await new HistoryCommand
            {
                UserId = r.UserId,
                IssueId = r.WorkflowId,
                ActionContent = $"Tài khoản {name} đã thêm  người dùng {nameFlow} vào công văn ${subjectWorkDispatch}"
            }.ExecuteAsync();
        }


        await SendAsync(result);
    }
}
