namespace WorkManagementSystem.Features.TaskDetail.UpdateProgressTask;

public class Endpoint : Endpoint<Request, ResultModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/task/update-progressTask");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.UpdateProgressTask(r);
        await SendAsync(ResultModel<string>.Create(result));
    }
}
