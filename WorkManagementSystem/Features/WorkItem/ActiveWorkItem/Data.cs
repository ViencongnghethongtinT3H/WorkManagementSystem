using System.Globalization;
using System.Security.Cryptography;

namespace WorkManagementSystem.Features.WorkItem.ActiveWorkItem;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> ActiveWorkItem(Request r)
    {
        var workItemRepository = _unitOfWork.GetRepository<Entities.WorkItem>();

        var workItem = await workItemRepository.GetAsync(r.WorkItemId);
        var implementRepository = _unitOfWork.GetRepository<Implementer>();
        var implementOld = await implementRepository.FindBy(x => x.IssuesId == r.WorkItemId).ToListAsync();

        if (workItem == null)
        {
            throw new Exception("Không tìm thấy");
        }
        int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);

        string code = randomNumber.ToString("D6", CultureInfo.InvariantCulture);

        workItem.ProcessingStatus = ProcessingStatusEnum.Processing;
        workItem.DateArrival = DateTime.Now;
        workItem.WorkItemNumber = code;
        workItem.Updated = DateTime.Now;    
        workItem.Dealine = r.Dealine;
        workItem.EvictionTime = r.EvictionTime;
        workItem.UserId = r.UserId;
        workItemRepository.Update(workItem);
        implementRepository.HardDeletes(implementOld);
        var implements = ToImplementer(r, r.WorkItemId);
        await implementRepository.AddRangeAsync(implements);
        await _unitOfWork.CommitAsync();
        return workItem.Id.ToString();

    }

    private List<Implementer> ToImplementer(Request r, Guid IssuesId)
    {
        var lst = new List<Implementer>();
        foreach (var item in r.Implementers)
        {
            lst.Add(new Implementer
            {
                IssuesId = IssuesId,
                ProgressValue = ProgressValueEnum.Progress0,
                IsTaskItem = false,
                UserReceiveId = item.UserReceiveId,
                Note = item.Note,
                DepartmentReceiveId = item.DepartmentReceiveId
            });
        }
        return lst;
    }
}