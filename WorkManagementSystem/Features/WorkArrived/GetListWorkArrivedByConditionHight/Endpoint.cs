namespace WorkManagementSystem.Features.WorkArrived.GetListWorkArrivedByConditionHight;

public class Endpoint : Endpoint<InputRequest, ListResultModel<WorkArriveResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("workArrived/get-list-work-arrived-by-condition-hight");
    }

    public override async Task HandleAsync(InputRequest query, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetListWorkDispatchWattingWork(query));
    }
}
