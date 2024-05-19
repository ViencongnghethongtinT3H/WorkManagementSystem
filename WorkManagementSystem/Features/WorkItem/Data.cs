namespace WorkManagementSystem.Features.WorkItem;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateWorkItems(Entities.WorkItem workItem)
    {
        var workItemRepository = _unitOfWork.GetRepository<Entities.WorkItem>();
        workItemRepository.Add(workItem);
        await _unitOfWork.CommitAsync();
        return workItem.Id.ToString();

    }
}
