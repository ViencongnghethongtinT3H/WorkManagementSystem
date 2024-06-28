using System.Globalization;
using System.Security.Cryptography;

namespace WorkManagementSystem.Features.WorkDispatch.PublishWorkDispatch;

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

        int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
        workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
        workItem.WorkflowStatus = WorkflowStatusEnum.WaittingWorkArrived;
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

                await new SendEmailCommand
                {
                    toEmail = acc.Email,
                    //FileNames = fileName,
                    body = "232",
                    subject = "Thông báo về công văn đến"
                }.ExecuteAsync();
            }
            await company.AddRangeAsync(lst);


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
