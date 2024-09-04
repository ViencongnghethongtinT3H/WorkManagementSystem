namespace WorkManagementSystem.Features.Meeting.DeleteMeeting
{
    public class EndPoint : Endpoint<Request, ResultModel<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EndPoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override void Configure()
        {
            AllowAnonymous();
            Get("/Meeting/delete-meeting-by-id");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var result = await data.DeleteMeeting(r);
            // Xử lý notification
          
            var name = await new GetUserNameCommand { UserId = r.UserId }.ExecuteAsync();
            //var lstcmd = new List<NotificationCommandbase>();
            //lstcmd.Add(new NotificationCommandbase
            //{
            //    Content = $"Tài khoản {name} đã tạo cuộc họp vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
            //    UserReceive = r.UserId,
            //    UserSend = r.UserId,
            //    Url = r.Id.ToString(),
            //    NotificationType = NotificationType.WorkItem,
            //    NotificationWorkItemType = NotificationWorkItemType.SendWorkItem
            //});

            //await new LstNotificationCommand
            //{
            //    NotificationCommands = lstcmd
            //}.ExecuteAsync();

            // Xử lý lưu lịch sử
            await new HistoryCommand
            {
                UserId = r.UserId,
                IssueId = new Guid(r.Id.ToString()),
                ActionContent = $"Tài khoản {name} đã xóa cuộc họp"
            }.ExecuteAsync();



            await SendAsync(result);
        }
    }
}
