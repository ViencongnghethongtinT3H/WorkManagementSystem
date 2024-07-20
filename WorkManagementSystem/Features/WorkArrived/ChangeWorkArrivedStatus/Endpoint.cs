namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
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
            Post("/workArrived/change-work-arrived-status");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var data = new Data(_unitOfWork);
            var result = await data.ChangeWorkArrivedStatus(r);
            var name = await new GetUserNameCommand
            {
                UserId = r.UserIds.FirstOrDefault()
            }.ExecuteAsync();
            // láy ra subject cua cong van
            var notationWorkDispatch = await new GetNotationWorkDispatchCommand
            {
                WorkDispatchId = r.WorkArriveId
            }.ExecuteAsync();
            var lstcmd = new List<NotificationCommandbase>();
            // notifine
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} {r.ActionType.GetDescription()} của công văn {notationWorkDispatch} vào {DateTime.Now.ToFormatString("dd/MM/yyyy hh:mm")}",
                UserReceive = r.UserIds.FirstOrDefault(),
                UserSend = r.UserIds.FirstOrDefault(),
                Url = r.UserIds.FirstOrDefault().ToString(),
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
                IssueId = r.WorkArriveId,
                ActionContent = $"Tài khoản {name} {r.ActionType.GetDescription()} {notationWorkDispatch} "
            }.ExecuteAsync();
            await SendAsync(result);
        }
    }
}
