using WorkManagementSystem.Entities;

namespace WorkManagementSystem.Features.TaskDetail.UpdateProgressTask;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> UpdateProgressTask(Request r)
    {
        var task = await _unitOfWork.GetRepository<Entities.TaskDetail>().GetAsync(r.TaskId);
        var taskRepository = _unitOfWork.GetRepository<Entities.TaskDetail>();

        if (task is not null)
        {
            var name = await new GetUserNameCommand
            {
                UserId = r.UserId,
            }.ExecuteAsync();

            if (task.WorkItemId.HasValue)
            {
                var content = string.Empty;
                if (task.ProcessingStatus == ProcessingStatusEnum.None)
                {
                    task.ProcessingStatus = ProcessingStatusEnum.Processing;
                    content = $"Tài khoản {name} đã bắt đầu xử lý công văn này";
                }

                else if (r.ProgressValue == 100)
                {
                    task.ProcessingStatus = ProcessingStatusEnum.Completed;
                    content = $"Tài khoản {name} đã hoàn thành nhiệm vụ này";
                }
                else
                {
                    content = $"Tài khoản {name} cập nhật tiến độ công việc {r.ProgressValue}%";
                }
                task.Updated = DateTime.Now;
                taskRepository.Update(task);

                await new HistoryCommand
                {
                    UserId = r.UserId,
                    IssueId = r.TaskId,
                    ActionContent = content
                }.ExecuteAsync();

            }
            var implemenRepo = _unitOfWork.GetRepository<Implementer>();
            var imple = await implemenRepo.GetAll().FirstOrDefaultAsync(x => x.IssuesId == task.Id
                && x.UserReceiveId == r.UserId);
            if (imple is not null)
            {
                imple.ProgressValue = r.ProgressValue;
                imple.Note = r.Note;
                imple.Updated = DateTime.Now;
            }
            implemenRepo.Update(imple);
            await _unitOfWork.CommitAsync();


            var lstcmd = new List<NotificationCommandbase>();

            lstcmd.Add(new NotificationCommandbase
            {
                Content = $"Tài khoản {name} cập nhật tiến độ công việc {r.ProgressValue}%",
                UserReceive = task.UserCreateTaskId,
                UserSend = r.UserId,
                Url = task.Id.ToString(),
                NotificationType = NotificationType.Task,
                NotificationWorkItemType = NotificationWorkItemType.UpdateProgressTask
            });

            await new LstNotificationCommand
            {
                NotificationCommands = lstcmd
            }.ExecuteAsync();
            return "Đã cập nhật thành công";
        }
        return "Không tìm thấy nhiệm vụ";
    }
}
