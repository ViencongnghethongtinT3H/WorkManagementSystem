namespace WorkManagementSystem.Features.Notification.GetNotificationByUserId;

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
        Get("/notification/get-notifications");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.GetNotification(r);
        await SendAsync(ResultModel<Response>.Create(result));
    }
}
