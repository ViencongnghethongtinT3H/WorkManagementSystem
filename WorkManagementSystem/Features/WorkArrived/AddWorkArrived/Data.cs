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
            workItem.WorkArrivedStatus = WorkArrivedStatus.Waitting;
            var userWorkRepo = _unitOfWork.GetRepository<UserWorkflow>();
            try
            {
                if (r.Id != null)
                {
                    workItem.Id = r.Id.Value;
                    workArrivedRepository.Update(workItem);

                }
                else
                {
                    workArrivedRepository.Add(workItem);
                    var userCompile = new UserWorkflow
                    {
                        WorkflowId = workItem.Id,
                        UserId = r.UserCompile,
                        UserWorkflowType = UserWorkflowType.Implementer,   // người thực hiện chính là người biên soạn
                        UserWorkflowStatus = UserWorkflowStatusEnum.Waitting,
                        Note = $"{await new GetUserNameCommand { UserId = r.UserCompile }.ExecuteAsync()} đã khởi tạo công văn đến vào {DateTime.Now.ToFormatString("dd/MM/yyyy")}"

                    };

                    var leaderShip = new UserWorkflow
                    {
                        WorkflowId = workItem.Id,
                        UserId = r.LeadershipDirectId,
                        UserWorkflowType = UserWorkflowType.Followers,
                        UserWorkflowStatus = UserWorkflowStatusEnum.Waitting,
                        Note = $"{await new GetUserNameCommand { UserId = r.LeadershipDirectId }.ExecuteAsync()} đã khởi tạo công văn đến vào {DateTime.Now.ToFormatString("dd/MM/yyyy")}"
                    };
                    await userWorkRepo.AddAsync(userCompile);
                    await userWorkRepo.AddAsync(leaderShip);
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
