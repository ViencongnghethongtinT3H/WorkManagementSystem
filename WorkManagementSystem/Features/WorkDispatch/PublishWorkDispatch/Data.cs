using System.Globalization;
using System.Security.Cryptography;
using static iTextSharp.text.pdf.AcroFields;
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
        List<string> FileNames = new List<string>();
        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var companyRepository = _unitOfWork.GetRepository<DispatchReceiveCompany>();
        var workArriveWattingRepo = _unitOfWork.GetRepository<Entities.WorkArriveWatting>();
        var companyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>();
        var fileManagerRepo = _unitOfWork.GetRepository<FileManagement>();
        int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
        workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
       
        
        var workDispatch = await workDispatchRepository.FindBy(p => p.Id == r.workDispatchId).FirstOrDefaultAsync();


        if (workDispatch is not null)
        {
            // cap nhay lai trang thai cua cong van di
            workDispatch.WorkItemNumber = workItem.WorkItemNumber;
            workDispatch.WorkflowStatus = WorkflowStatusEnum.Done;
            workDispatchRepository.Update(workDispatch);
           
            // Lưu file
            var folder = new FileManagement()
            {
                Created = DateTime.Now,
                FileManagementType = FileManagementType.WorkItem,
                Name = workDispatch.WorkItemNumber,
                UserId = r.UserCompile,
                ParentId = null,
            };
            await fileManagerRepo.AddAsync(folder);
            if (r.FileAttachIds.IsAny())
            {
                var filesRepo = _unitOfWork.GetRepository<FileAttach>();
                var files = await filesRepo.GetAll().Where(x => r.FileAttachIds.Contains(x.Id)).ToListAsync();
                foreach (var item in files)
                {
                    item.IssuesId = workItem.Id;
                    item.Updated = DateTime.Now;
                    item.RefId = folder.Id;
                    filesRepo.Update(item);
                    FileNames.Add(item.FileName);
                    
                }
            }
        }
        // them moi cong van vao danh sách chờ
        // them moi cong van vao danh sách chờ
        workItem.Notation = workDispatch.Notation;
        workItem.ItemId = workDispatch.ItemId;
        workItem.IndustryId = workDispatch.IndustryId;
        workItem.WorkItemNumber = workDispatch.WorkItemNumber;
        workItem.WorkflowStatus = WorkflowStatusEnum.WaittingWorkArrived;
        await workArriveWattingRepo.AddAsync(workItem);
        var lst = new List<DispatchReceiveCompany>();

        // Lấy ra email đơn vị nhận
        var receiveRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>().GetAll();

        if (r.ReceiveCompanys.IsAny())
        {
            foreach (var item in r.ReceiveCompanys)
            {
                var company = await companyRepository.GetAll().Where(p=>p.AccountReceiveId == item.Id).FirstOrDefaultAsync();
                if (company != null)
                {
                    lst.Add(new DispatchReceiveCompany
                    {
                        WorkDispatchId = workItem.Id,
                        AccountReceiveId = item.Id,
                    });

                    var acc = await companyRepo.GetAll().FirstOrDefaultAsync(p => p.Id == company.AccountReceiveId);
                    if (acc != null)
                    {
                        await new SendEmailCommand
                        {
                            toEmail = "vansy9x@gmail.com",
                            FileNames = FileNames,
                            body = "Công văn đi, kính gửi các đơn vị thành viên",
                            subject = "Thông báo về công văn đến"
                        }.ExecuteAsync();
                    }
                   
                }           
            }
            
            await companyRepository.AddRangeAsync(lst);
        }
        //Todo: Insert vào 1 bảng mới
        await _unitOfWork.CommitAsync();
        return workItem.Id.ToString();
    }

}
