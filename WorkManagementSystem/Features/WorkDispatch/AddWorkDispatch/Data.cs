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
        var implementRepository = _unitOfWork.GetRepository<Implementer>();
        var workDispatchRepository = _unitOfWork.GetRepository<Entities.WorkDispatch>();

        if(r.Id != null)
        {
            workItem.Id = r.Id.Value;
            workDispatchRepository.Update(workItem);
        }
        else
        {
            workItem.WorkflowStatus = WorkflowStatusEnum.Waitting;
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
            var userWorkRepo = _unitOfWork.GetRepository<UserWorkflow>();

            var userCompile = new UserWorkflow
            {
                WorkflowId = workItem.Id,
                UserId = r.UserCompile,
                UserWorkflowType = UserWorkflowType.Implementer,   // người thực hiện chính là người biên soạn
                UserWorkflowStatus = UserWorkflowStatusEnum.Waitting,
                Note = $"{await new GetUserNameCommand { UserId = r.UserCompile }.ExecuteAsync()} đã khởi tạo công văn vào {DateTime.Now.ToFormatString("dd/MM/yyyy")}"

            };

            var leaderShip = new UserWorkflow
            {
                WorkflowId = workItem.Id,
                UserId = r.LeadershipDirectId,
                UserWorkflowType = UserWorkflowType.Followers,
                UserWorkflowStatus = UserWorkflowStatusEnum.Waitting,
                Note = $"{await new GetUserNameCommand { UserId = r.LeadershipDirectId }.ExecuteAsync()} đã khởi tạo công văn vào {DateTime.Now.ToFormatString("dd/MM/yyyy")}"
            };

            await userWorkRepo.AddAsync(userCompile);
            await userWorkRepo.AddAsync(leaderShip);
        }
        await _unitOfWork.CommitAsync();
        return workItem.Id.ToString();
    }
}
