using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch;

// Chức năng chuyển công văn cho người/n-người theo dõi (bật popup lên cho chọn người theo dõi công văn và có thêm phần vai trò:  Người theo dõi, Người phối hợp/ xử lý)
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
        Post("/workDispatch/add-user-to-work-dispatch");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork, _eventImplement);
        ResultModel<bool>? result= await data.AddUserToWorkDispatch(r);
        
        foreach (var item in r.UserIds)
        {
            var name = await data.GetUserName(item);
            //var receiveName = await new GetUserNameCommand
            //{
            //    UserId = item,
            //}.ExecuteAsync();
            //
            //foreach (var notification in userNotifications)
            //{
            //    lstcmd.Add(new NotificationCommandbase
            //    {
            //        Content = $"Tài khoản {name}  đã thêm người dùng {receiveName} theo dõi vào công văn",
            //        UserReceive = notification,
            //        UserSend = item,
            //        Url = r.WorkflowId.ToString(),
            //        NotificationType = NotificationType.WorkItem,
            //        NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
            //    });
            //}
            //await new LstNotificationCommand
            //{
            //    NotificationCommands = lstcmd
            //}.ExecuteAsync();

            await new HistoryCommand
            {
                UserId = item,
                IssueId = r.WorkflowId,
                ActionContent = $"Tài khoản {name} đã thêm người dùng theo dõi vào công văn "
            }.ExecuteAsync();
          
        }
        
        // Thêm phần lịch sử

        // Thêm phần notification

        await SendAsync(result);
    }
}
