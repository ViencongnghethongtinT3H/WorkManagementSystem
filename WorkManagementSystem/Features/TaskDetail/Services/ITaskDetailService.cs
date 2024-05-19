namespace WorkManagementSystem.Features.TaskDetail.Services;

public interface ITaskDetailService
{
    Task<string> SaveTaskDetail(CreateTaskDetail.Request r);
}
