namespace WorkManagementSystem.Features.WorkDispatch.GetWorkWattingById;

public class Endpoint : Endpoint<Request, ResultModel<WorkWattingArriveDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("/WorkArriveWatting/get-by-id");
    }

    public override async Task HandleAsync(Request query, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetWorkItemById(query));
    }
}
