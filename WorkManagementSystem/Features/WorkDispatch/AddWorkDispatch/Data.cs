namespace WorkManagementSystem.Features.WorkDispatch.AddWorkDispatch;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateWorkDispatch(Entities.WorkDispatch workItem, Request r)
    {
        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        workDispatchRepository.Add(workItem);

        if (r.FileAttachIds.IsAny())
        {
            var filesRepo = _unitOfWork.GetRepository<FileAttach>();
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
        var user = await _unitOfWork.GetRepository<Entities.User>().GetAsync(r.UserCompile);
        if (user is not null)
        {
            return user.Name;
        }
        return string.Empty;

    }
}
