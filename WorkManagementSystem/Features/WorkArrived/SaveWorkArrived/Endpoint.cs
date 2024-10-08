namespace WorkManagementSystem.Features.WorkArrived.SaveWorkArrived
{
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
            Post("/workArrived/save-work-arrived");
        }
        public override async Task HandleAsync(Request r, CancellationToken c)
        {

            // Xử lý notification
            var lstcmd = new List<NotificationCommandbase>();
            var name = await new GetUserNameCommand { UserId = r.UserId }.ExecuteAsync();
            var notationWorkDispatch = await new GetNotationWorkDispatchCommand
            {
                WorkDispatchId = r.WorkArriveId,
            }.ExecuteAsync();

            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} đã lưu công văn {notationWorkDispatch} vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
                UserReceive = r.UserId,
                UserSend = r.UserId,
                Url = r.WorkArriveId.ToString(),
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
                UserId = r.UserId,
                IssueId = r.WorkArriveId,
                ActionContent = $"Tài khoản {name} đã lưu {notationWorkDispatch}"
            }.ExecuteAsync();

            var data = new Data(_unitOfWork);
            ResultModel<bool>? result = await data.SaveWorkArrived(r);
            await SendAsync(result);
        }
    }
}
