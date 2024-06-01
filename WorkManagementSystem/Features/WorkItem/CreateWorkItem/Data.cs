namespace WorkManagementSystem.Features.WorkItem.CreateWorkItem;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateWorkItems(Entities.WorkItem workItem, Request r)
    {
        var workItemRepository = _unitOfWork.GetRepository<Entities.WorkItem>();
        workItemRepository.Add(workItem);

        if (r.FileAttachIds.IsAny())
        {
            var filesRepo = _unitOfWork.GetRepository<Entities.FileAttach>();
            var files = await filesRepo.GetAll().Where(x => r.FileAttachIds.Contains(x.Id)).ToListAsync();
            foreach (var item in files)
            {
                item.IssuesId = workItem.Id;
                item.Updated = DateTime.Now;
                filesRepo.Update(item);
            }
        }

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
