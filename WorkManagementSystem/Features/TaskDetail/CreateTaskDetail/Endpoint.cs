using WorkManagementSystem.Features.TaskDetail.Services;
using WorkManagementSystem.Shared.Dtos;

namespace WorkManagementSystem.Features.TaskDetail.CreateTaskDetail;

public class Endpoint : Endpoint<Request, ResultModel<Response>>
{
    private readonly ITaskDetailService _taskDetailService;

    public Endpoint(ITaskDetailService taskDetailService)
    {
        _taskDetailService = taskDetailService;
    }
    public override void Configure()
    {
        Post("task/create-task");
        AllowAnonymous();
        AccessControl(
            keyName: "Article_Save_Own",
            behavior: Apply.ToThisEndpoint,
            groupNames: "task");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var result = ResultModel<Response>.Create(new Response
        {
            TaskId = await _taskDetailService.SaveTaskDetail(r)
        });

        if (string.IsNullOrEmpty(result.Data.TaskId))
            ThrowError("Không thể thêm nhiệm vụ");

        await SendAsync(result);
    }
}
