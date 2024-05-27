namespace WorkManagementSystem.Features.WorkItem.CreateWorkItem;

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

    public async Task<string> GetUserName(Request r)
    {
        var user = await _unitOfWork.GetRepository<Entities.User>().GetAsync(r.UserCreatedId);
        if (user is not null)
        {
            return user.Name;
        }
        return string.Empty;

    }
}
