using WorkManagementSystem.Entities;

namespace WorkManagementSystem.Features.TaskDetail.Services;

public class TaskDetailService : ITaskDetailService
{
    private readonly IUnitOfWork _unitOfWork;
    public TaskDetailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<string> SaveTaskDetail(CreateTaskDetail.Request r)
    {
        var taskRepository = _unitOfWork.GetRepository<Entities.TaskDetail>();
        var taskDetail = ToEntity(r);
        taskDetail.DepartmentSentId = r.UserCreateTaskId;

        taskRepository.Add(taskDetail);
        var implementRepository = _unitOfWork.GetRepository<Entities.Implementer>();
        var implements = ToImplementer(r, taskDetail.Id);

        await implementRepository.AddRangeAsync(implements);
        
        await _unitOfWork.CommitAsync();



        var name = await new GetUserNameCommand
        {
            UserId = r.UserCreateTaskId,
        }.ExecuteAsync();
        var lstcmd = new List<NotificationCommandbase>();
        foreach (var notification in implements)
        {
            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} đã tạo một nhiệm vụ cho bạn, vui lòng hoàn thành đúng hạn",
                UserReceive = notification.UserReceiveId,
                UserSend = r.UserCreateTaskId,
                Url = taskDetail.Id.ToString(),
                NotificationType = NotificationType.Task,
                NotificationWorkItemType = NotificationWorkItemType.SendTask
            });
        }
        await new LstNotificationCommand
        {
            NotificationCommands = lstcmd
        }.ExecuteAsync();

        await new HistoryCommand
        {
            UserId = r.UserCreateTaskId,
            IssueId = taskDetail.Id,
            ActionContent = $"Tài khoản {name} tạo một nhiệm vụ"
        }.ExecuteAsync();


        return taskDetail.Id.ToString();
    }
    public Entities.TaskDetail ToEntity(CreateTaskDetail.Request r) => new()
    {
        KeyWord = r.KeyWord,
        DepartmentReceiveId = r.DepartmentReceiveId,
        Content = r.Content,
        Priority = r.Priority,
        Periodical = r.Periodical,
        WorkItemId = r.WorkItemId,
        DealinePeriodical = r.DealinePeriodical,
        IsPeriodical = r.IsPeriodical,
        LeadershipDirectId = r.LeadershipDirectId,
        UserCreateTaskId = r.UserCreateTaskId,
        ProcessingStatus = ProcessingStatusEnum.None,
        Dealine = r.Dealine,
        Status = StatusEnum.Active,
        UserIdCreated = r.UserCreateTaskId.ToString(),
    };
    public List<Entities.Implementer> ToImplementer(CreateTaskDetail.Request r, Guid IssuesId)
    {
        var lst = new List<Entities.Implementer>();
        foreach (var item in r.Implementers)
        {
            lst.Add(new Implementer
            {
                IssuesId = IssuesId,
                ProgressValue = ProgressValueEnum.Progress0,
                IsTaskItem = true,
                UserReceiveId = item.UserReceiveId,
                Note = item.Note

            });
        }
        return lst;
    }

}
