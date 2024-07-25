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
        List<string> FileNames = new List<string>();
        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var companyRepository = _unitOfWork.GetRepository<DispatchReceiveCompany>();
        var workArriveWattingRepo = _unitOfWork.GetRepository<Entities.WorkArriveWatting>();
        var companyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>();
        var fileManagerRepo = _unitOfWork.GetRepository<FileManagement>();
        var userWorkFlowRepo = _unitOfWork.GetRepository<UserWorkflow>();
        var filesRepo = _unitOfWork.GetRepository<FileAttach>();

        int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
        workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);

        var workDispatch = await workDispatchRepository.FindBy(p => p.Id == r.workDispatchId).FirstOrDefaultAsync();
        if (workDispatch is not null)
        {
            // cap nhay lai trang thai cua cong van di
            workDispatch.WorkItemNumber = workItem.WorkItemNumber;
            workDispatch.WorkflowStatus = WorkflowStatusEnum.Done;
            workDispatchRepository.Update(workDispatch);

            // update userWorkFlow
            var userWorkFlow = await userWorkFlowRepo.GetAll().FirstOrDefaultAsync(p => p.WorkflowId == r.workDispatchId && p.UserId == r.UserCompile);
            if (userWorkFlow is not null)
            {
                userWorkFlow.Note = $"Tài khoản {await new GetUserNameCommand { UserId = r.LeadershipDirectId }.ExecuteAsync()} đã phát hành công văn {await new GetNotationWorkDispatchCommand { WorkDispatchId = r.workDispatchId.Value }.ExecuteAsync()}";
                userWorkFlow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
                userWorkFlowRepo.Update(userWorkFlow);
            }
            // lấy ra toàn bộ các user đang theo dõi công văn 
            var userWorks = await userWorkFlowRepo.GetAll().AsNoTracking().Where(p => p.WorkflowId == r.workDispatchId).ToListAsync();

            foreach (var userWork in userWorks)
            {
                // tìm folder "công văn"
                var parnetFolder = await fileManagerRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userWork.UserId && p.Name == "Công văn đi");

                if (parnetFolder is not null)
                {
                    // kiem tra xem co folder dc tao ra tu WorkItemNumber hay chua

                       var folder = new FileManagement()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.Now,
                            FileManagementType = FileManagementType.WorkItem,
                            Name = workItem.WorkItemNumber,
                            UserId = userWork.UserId,
                            ParentId = parnetFolder is not null ? parnetFolder.Id : null,
                        };
                        await fileManagerRepo.AddAsync(folder);
                    

                    if (r.Files.IsAny())
                    {
                        // lưu file
                        var fileIds = r.Files.Select(p => p.fileId);
                        var files = await filesRepo.GetAll().Where(x => fileIds.Contains(x.Id)).ToListAsync();
                        foreach (var item in files)
                        {
                            // check file của công văn
                            var checkFile = files.Where(p => p.IssuesId == workDispatch.Id);
                            if (checkFile.Any())
                            {
                                item.IssuesId = userWork.WorkflowId;
                                item.Updated = DateTime.Now;
                                item.RefId = folder.Id;
                                filesRepo.Update(item);
                            }
                            else
                            {
                                item.Id = Guid.NewGuid();
                                item.IssuesId = userWork.WorkflowId;
                                item.Created = DateTime.Now;
                                item.RefId = folder.Id;
                                await filesRepo.AddAsync(item);
                            }
                            FileNames.Add(item.FileName);
                        }
                    }
                }
            }
        }
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
                var company = await companyRepository.GetAll().Where(p => p.AccountReceiveId == item.Id).FirstOrDefaultAsync();
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

       
        await _unitOfWork.CommitAsync();
        return workItem.Id.ToString();
    }

}
