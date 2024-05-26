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
    
        
        if (task is not null)
        {
            var name = await new GetUserNameCommand
            {
                UserId = r.UserId,
            }.ExecuteAsync();

            if (task.WorkItemId.HasValue)
            {
                var workItemRepository = _unitOfWork.GetRepository<Entities.WorkItem>();
                var workItem = await workItemRepository.GetAsync(task.WorkItemId.Value);
                if (workItem != null && workItem.ProcessingStatus != ProcessingStatusEnum.Processing)
                {
                    workItem.ProcessingStatus = ProcessingStatusEnum.Processing;
                    workItem.Updated = DateTime.Now;
                    workItem.DateArrival = DateTime.Now;
                    workItemRepository.Update(workItem);

                    await new HistoryCommand
                    {
                        UserId = r.UserId,
                        IssueId = r.TaskId,
                        ActionContent = $"Tài khoản {name} đã bắt đầu xử lý công văn này"
                    }.ExecuteAsync();
                }
                
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

            
            await new HistoryCommand
            {
                UserId = r.UserId,
                IssueId = r.TaskId,
                ActionContent = $"Tài khoản {name} đã cập nhật tiến độ xử lý nhiệm vụ lên {r.ProgressValue.GetDescription()}"
            }.ExecuteAsync();

            return "Đã cập nhật thành công";
        }
        return "Không tìm thấy nhiệm vụ";
    }
}
