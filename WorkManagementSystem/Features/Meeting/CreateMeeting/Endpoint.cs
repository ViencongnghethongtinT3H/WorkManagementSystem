namespace WorkManagementSystem.Features.Meeting.CreateMeeting
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
            Post("/Meeting/create-update-meeting");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var a = await data.CreateMeeting(r);
            var result = ResultModel<Response>.Create(new Response
            {
                Id = a.Data
            });
            if (string.IsNullOrEmpty(result.Data.Id))
                ThrowError("lỗi");
            // Xử lý notification
            var lstcmd = new List<NotificationCommandbase>();
            var name = await new GetUserNameCommand { UserId = r.OrganizerId }.ExecuteAsync();
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} đã tạo {r.TypeMeeting.GetDescription()} vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
                UserReceive = r.OrganizerId,
                UserSend = r.OrganizerId,
                Url = result.Data.Id,
                NotificationType = NotificationType.WorkItem,
                NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
            });
            foreach (var item in r.UserMeetings)
            {
                lstcmd.Add(new NotificationCommandbase
                {
                    Content = $"{name} thêm {await new GetUserNameCommand { UserId = item.UserId }.ExecuteAsync()} vào {r.TypeMeeting.GetDescription()} vào lúc {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
                    UserReceive = item.UserId,
                    UserSend = r.OrganizerId,
                    Url = result.Data.Id,
                    NotificationType = NotificationType.WorkItem,
                    NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
                });
            }
            await new LstNotificationCommand
            {
                NotificationCommands = lstcmd
            }.ExecuteAsync();
            // Xử lý lưu lịch sử
            await new HistoryCommand
            {
                UserId = r.OrganizerId,
                IssueId = new Guid(result.Data.Id),
                ActionContent = $"Tài khoản {name} đã tạo ${r.TypeMeeting.GetDescription()}"
            }.ExecuteAsync();
            await SendAsync(result);

        }
    }
}
