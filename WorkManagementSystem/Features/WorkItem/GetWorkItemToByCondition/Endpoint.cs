namespace WorkManagementSystem.Features.WorkItem.GetWorkItemToByCondition;

public class Endpoint : Endpoint<Request, ListResultModel<WorkItemResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("WorkItem/get-work-incoming-by-condition");
    }

    public override async Task HandleAsync(Request query, CancellationToken c)
    {
        var queryModel = HttpContext.SafeGetListQuery<InputRequest, Response>(query.Query);
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetWorkItemByCondition(queryModel));
    }
}
