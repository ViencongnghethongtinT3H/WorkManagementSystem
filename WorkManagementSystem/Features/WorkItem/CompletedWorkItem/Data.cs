namespace WorkManagementSystem.Features.WorkItem.CompletedWorkItem;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CompletedWorkItem(Request r)
    {
        var workItemRepository = _unitOfWork.GetRepository<Entities.WorkItem>();
        var workItem = await workItemRepository.GetAsync(r.WorkItemId);
        if (workItem is not null)
        {
            workItem.ProcessingStatus = ProcessingStatusEnum.Completed;
            workItem.Updated = DateTime.Now;
            workItemRepository.Update(workItem);
            await _unitOfWork.CommitAsync();

            var name = await new GetUserNameCommand
            {
                UserId = new Guid(workItem.UserIdCreated),
            }.ExecuteAsync();

            await new HistoryCommand
            {
                UserId = new Guid(workItem.UserIdCreated),
                IssueId = workItem.Id,
                ActionContent = $"Tài khoản {name} hoàn thành công văn có số {workItem.WorkItemNumber}"
            }.ExecuteAsync();
            return true;
        }
        return false;
    }
}
