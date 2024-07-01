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

    public async Task<string> CreateWorkDispatch(Entities.WorkArriveWatting workItem, Request r)
    {

        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var workArriveWattingRepo = _unitOfWork.GetRepository<WorkArriveWatting>();
        int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
        workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
        workItem.WorkflowStatus = WorkflowStatusEnum.Done;
      //  workDispatchRepository.Update(workItem);

        // them moi cong van vao danh sách chờ
        //var 


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
        //Todo: Insert vào 1 bảng mới
        await _unitOfWork.CommitAsync();
        return workItem.Id.ToString();
    }

}
