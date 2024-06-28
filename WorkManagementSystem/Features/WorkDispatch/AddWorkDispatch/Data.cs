using System.Globalization;
using System.Security.Cryptography;
using WorkManagementSystem.Features.ToImplementer;
namespace WorkManagementSystem.Features.WorkDispatch.AddWorkDispatch;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventImplement _eventImplement;
    public Data(IUnitOfWork unitOfWork, IEventImplement eventImplement)
    {
        _unitOfWork = unitOfWork;
        _eventImplement = eventImplement;
    }

    public async Task<(string, List<Guid> UserReceiveNotification)> CreateWorkDispatch(Entities.WorkDispatch workItem, Request r)
    {
        var userNotifications = new List<Guid>();
        var implementRepository = _unitOfWork.GetRepository<Implementer>();
        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        if (r.IsPublish)
        {
            int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
            workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
            workItem.WorkflowStatus = WorkflowStatusEnum.Release;
        }
        else
        {
            workItem.WorkflowStatus = WorkflowStatusEnum.Waitting;
        }
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
        var company = _unitOfWork.GetRepository<DispatchReceiveCompany>();
        var lst = new List<DispatchReceiveCompany>();
        if (r.ReceiveCompanyIds.IsAny())
        {
            foreach (var item in r.ReceiveCompanyIds)
            {
                lst.Add(new DispatchReceiveCompany
                {
                    WorkDispatchId = workItem.Id,
                    AccountReceiveId = item
                });
            }
            await company.AddRangeAsync(lst);
        }
        //  var userWorkRepo = _unitOfWork.GetRepository<UserWorkflow>();

        //var use = new UserWorkflow
        //{
        //    WorkflowId = workItem.Id,
        //    UserId = workItem.Id,
        //    UserWorkflowType = UserWorkflowType.Implementer
        //};
        var implementOld = await implementRepository.FindBy(x => x.IssuesId == workItem.Id).ToListAsync();
        if(implementOld is not null)
        implementRepository.HardDeletes(implementOld);
        var implements = _eventImplement.ToImplementer(r.RequestImplementer, workItem.Id);
        await implementRepository.AddRangeAsync(implements);
        userNotifications = implements.Select(x => x.UserReceiveId).ToList();
        await _unitOfWork.CommitAsync();
        return (workItem.Id.ToString(), userNotifications);
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
