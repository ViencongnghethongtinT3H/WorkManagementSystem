namespace WorkManagementSystem.Features.WorkDispatch.GetListWorkDispatchByConditionHight;

public class Endpoint : Endpoint<InputRequest, ListResultModel<WorkDispatchResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("workDispatch/get-list-work-dispatch-by-condition-hight");
    }

    public override async Task HandleAsync(InputRequest query, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetListWorkDispatchWattingWork(query));
    }
}
