namespace WorkManagementSystem.Features.WorkItem.CompletedWorkItem;

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
        Post("/WorkItem/complete-work-item");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.CompletedWorkItem(r);
        await SendAsync(ResultModel<bool>.Create(result));
    }
}
