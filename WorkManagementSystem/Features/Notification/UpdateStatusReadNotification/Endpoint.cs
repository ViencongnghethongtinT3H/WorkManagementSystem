namespace WorkManagementSystem.Features.Notification.UpdateStatusReadNotification;

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
        Get("/notification/read-notifications-by-id");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.UpdateStatusReadNotification(r);
        await SendAsync(ResultModel<bool>.Create(result));
    }
}
