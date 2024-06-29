﻿namespace WorkManagementSystem.Features.WorkDispatch.AddWorkDispatch;

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
        Post("/WorkDispatch/create");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var userNotifications = new List<Guid>();
        var data = new Data(_unitOfWork);
        string workItemId = await data.CreateWorkDispatch(Map.ToEntity(r), r);

        // Xử lý notification
        var lstcmd = new List<NotificationCommandbase>();
        var name = await data.GetUserName(r);
        var receiveName = await new GetUserNameCommand
        {
            UserId = r.LeadershipDirectId
        }.ExecuteAsync();


        lstcmd.Add(new NotificationCommandbase
        {
            Content = $"Tài khoản {name} đã tạo một công văn do {receiveName} chỉ đạo. Bạn vui lòng kiểm tra",
            UserReceive = r.LeadershipDirectId,
            UserSend = r.UserCompile,
            Url = workItemId,
            NotificationType = NotificationType.WorkItem,
            NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
        });

        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();

        // Xử lý lưu lịch sử
        await new HistoryCommand
        {
            UserId = r.UserCompile,
            IssueId = new Guid(workItemId),
            ActionContent = $"Tài khoản {name} đã tạo thêm một công văn"
        }.ExecuteAsync();


        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = workItemId
        });

        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể thêm công văn");
        await SendAsync(result);

    }
}
