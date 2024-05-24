namespace WorkManagementSystem.Features.TaskDetail.GetTaskDetailById;

public class Endpoint : Endpoint<Request, ResultModel<TaskDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("task/get-task-by-id");
    }

    public override async Task HandleAsync(Request query, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetTaskItemById(query));
    }
}
