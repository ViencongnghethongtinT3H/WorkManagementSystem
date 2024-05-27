namespace WorkManagementSystem.Features.TaskDetail.GetTaskInCommingByCondition;

public class Endpoint : Endpoint<Request, ListResultModel<TaskDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("task/get-task-incoming-by-condition");
    }

    public override async Task HandleAsync(Request query, CancellationToken c)
    {
        var queryModel = HttpContext.SafeGetListQuery<InputRequest, Response>(query.Query);
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetTaskByCondition(queryModel));
    }
}
