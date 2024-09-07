namespace WorkManagementSystem.Features.Meeting.UpdateStatusUsersMeeting
{
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
            Post("/Meeting/update-status-user-meeting");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var a = await data.UpdateStatusMeeting(r);
            var result = ResultModel<Response>.Create(new Response
            {
                message = a.Data
            });
            if (string.IsNullOrEmpty(result.Data.message))
                ThrowError("lỗi");
            // Xử lý notification
            var lstcmd = new List<NotificationCommandbase>();
            var name = await new GetUserNameCommand { UserId = r.UserId }.ExecuteAsync();
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} đã tham gia {r.TypeMeeting.GetDescription()} vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
                UserReceive = r.OrganizerId,
                UserSend = r.UserId,
                Url = result.Data.message,
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
                UserId = r.OrganizerId,
                IssueId = new Guid(result.Data.message),
                ActionContent = $"Tài khoản {name} tham gia vào ${r.TypeMeeting.GetDescription()}"
            }.ExecuteAsync();
            await SendAsync(result);

        }
    }
}
