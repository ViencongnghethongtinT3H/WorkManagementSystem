using System.Globalization;
using System.Security.Cryptography;
using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.PublishWorkDispatch;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventImplement _eventImplement;
    public Data(IUnitOfWork unitOfWork, IEventImplement eventImplement)
    {
        _unitOfWork = unitOfWork;
        _eventImplement = eventImplement;
    }

    public async Task<string> CreateWorkDispatch(Entities.WorkDispatch workItem, Request r)
    {
        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var userNotifications = new List<Guid>();
        int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
        workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
        workItem.WorkflowStatus = WorkflowStatusEnum.WaittingWorkArrived;
        workDispatchRepository.Add(workItem);
        List<string> FileNames = new List<string>();
        var implementRepository = _unitOfWork.GetRepository<Implementer>();
        if (r.FileAttachIds.IsAny())
        {
            var filesRepo = _unitOfWork.GetRepository<FileAttach>();
            var files = await filesRepo.GetAll().Where(x => r.FileAttachIds.Contains(x.Id)).ToListAsync();
            foreach (var item in files)
            {
                item.IssuesId = workItem.Id;
                item.Updated = DateTime.Now;
                filesRepo.Update(item);
                FileNames.Add(item.FileName);
            }
        }
        var company = _unitOfWork.GetRepository<DispatchReceiveCompany>();
        var lst = new List<DispatchReceiveCompany>();

        // lấy ra tên file
      

        // Lấy ra email đơn vị nhận
        var receiveRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>().GetAll();

        if (r.ReceiveCompanyIds.IsAny())
        {
            foreach (var item in r.ReceiveCompanyIds)
            {
                lst.Add(new DispatchReceiveCompany
                {
                    WorkDispatchId = workItem.Id,
                    AccountReceiveId = item
                });

                var acc = await receiveRepo.FirstOrDefaultAsync(x => x.Id == item);
                if(acc is not null)
                {
                    await new SendEmailCommand
                    {
                        toEmail = acc.Email,
                        FileNames = FileNames,
                        body = "232",
                        subject = "Thông báo về công văn đến"
                    }.ExecuteAsync();
                }
                
            }
            await company.AddRangeAsync(lst);
        }
        var implementOld = await implementRepository.FindBy(x => x.IssuesId == workItem.Id).ToListAsync();
        if (implementOld is not null)
        implementRepository.HardDeletes(implementOld);
        var implements = _eventImplement.ToImplementer(r.RequestImplementer, workItem.Id);
        await implementRepository.AddRangeAsync(implements);
        userNotifications = implements.Select(x => x.UserReceiveId).ToList();
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
