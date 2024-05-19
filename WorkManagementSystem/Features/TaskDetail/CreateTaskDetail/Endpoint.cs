using WorkManagementSystem.Features.TaskDetail.Services;

namespace WorkManagementSystem.Features.TaskDetail.CreateTaskDetail;

public class Endpoint : Endpoint<Request, Response>
{
    private readonly ITaskDetailService _taskDetailService;

    public Endpoint(ITaskDetailService taskDetailService)
    {
        _taskDetailService = taskDetailService;
    }
    public override void Configure()
    {
        Post("api/task/save-task");
        AllowAnonymous();

        // Claims(Claim.AuthorID);
        AccessControl(
            keyName: "Article_Save_Own",
            behavior: Apply.ToThisEndpoint,
            groupNames: "task");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        
        Response.TaskId = await _taskDetailService.SaveTaskDetail(r);

        if (string.IsNullOrEmpty(Response.TaskId))
            ThrowError("Không thể thêm nhiệm vụ");

        await SendAsync(Response);
    }
}
