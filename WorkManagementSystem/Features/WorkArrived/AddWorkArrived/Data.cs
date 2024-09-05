namespace WorkManagementSystem.Features.WorkArrived.AddWorkArrived
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> CreateWorkArrived(Entities.WorkArrived workItem, Request r)
        {
            var workArrivedRepository = _unitOfWork.GetRepository<Entities.WorkArrived>();
            var userWorkRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var stepRepo = _unitOfWork.GetRepository<WorkflowStep>();
            var workArriveWattingRepo = _unitOfWork.GetRepository<Entities.WorkArriveWatting>();

            try
            {
                if (r.Id != null)
                {
                    workItem.Id = r.Id.Value;
                    workArrivedRepository.Update(workItem);
                }
                else
                {

                    workItem.WorkArrivedStatus = WorkArrivedStatus.Waitting;
                    workArrivedRepository.Add(workItem);
                    if (r.FileAttachIds.IsAny())
                    {
                        var filesRepo = _unitOfWork.GetRepository<FileAttach>();
                        var files = await filesRepo.GetAll().Where(x => r.FileAttachIds.Contains(x.Id)).ToListAsync();
                        foreach (var item in files)
                        {
                            item.Id = Guid.NewGuid();
                            item.Created = DateTime.Now;
                            item.IssuesId = workItem.Id;
                            item.RefId = new Guid();
                            await filesRepo.AddAsync(item);
                        }
                    }

                    var stepUserCompile = new WorkflowStep
                    {
                        Step = StepEnum.ManagerApprove,
                        Note = "Lãnh đạo phê duyệt",
                        UserConfirm = r.UserCompile,
                        WorkflowId = workItem.Id,
                    };
                    var stepUserLeadershipDirect = new WorkflowStep
                    {
                        Step = StepEnum.ManagerApprove,
                        Note = "Lãnh đạo phê duyệt",
                        UserConfirm = r.LeadershipDirectId,
                        WorkflowId = workItem.Id,
                    };

                    await stepRepo.AddAsync(stepUserCompile);
                    await stepRepo.AddAsync(stepUserLeadershipDirect);
                    var notationWorkDispatch = await new GetNotationWorkDispatchCommand {WorkDispatchId = workItem.Id}.ExecuteAsync();
                    var userCompile = new UserWorkflow
                    {
                        WorkflowId = workItem.Id,
                        UserId = r.UserCompile,
                        UserWorkflowType = UserWorkflowType.Followers,   // người thực hiện chính là người biên soạn
                        UserWorkflowStatus = UserWorkflowStatusEnum.Done,
                        Note = $"{await new GetUserNameCommand { UserId = r.UserCompile }.ExecuteAsync()} đã khởi tạo công văn đến {notationWorkDispatch}",
                        UserCompile = r.UserCompile,
                    };

                    var leaderShip = new UserWorkflow
                    {
                        WorkflowId = workItem.Id,
                        UserId = r.LeadershipDirectId,
                        UserWorkflowType = UserWorkflowType.Submit,
                        UserWorkflowStatus = UserWorkflowStatusEnum.Waitting,
                        Note = $"{await new GetUserNameCommand { UserId = r.LeadershipDirectId }.ExecuteAsync()} đã được theo dõi công văn đến {notationWorkDispatch}",
                        UserCompile = r.UserCompile,
                    };
                    await userWorkRepo.AddAsync(userCompile);
                    await userWorkRepo.AddAsync(leaderShip);
                    var workArriveWatting = await workArriveWattingRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.IdworkArriveWatting);
                    workArriveWatting.WorkflowStatus = WorkflowStatusEnum.Done;
                    workArriveWattingRepo.Update(workArriveWatting);
                }
                await _unitOfWork.CommitAsync();
                return workItem.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
