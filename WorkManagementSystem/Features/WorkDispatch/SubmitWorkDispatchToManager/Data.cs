namespace WorkManagementSystem.Features.WorkDispatch.SubmitWorkDispatchToManager
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> UpdateStatusWhenSubmitWorkDispatch(Request r)
        {
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
            var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();
            try
            {
                var user = await userRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.UserId);
                if (user is null)
                {
                    return new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = "Không tìm thông tin người dùng!",
                        IsError = true,
                    };
                }
                var userWorkFlow = new UserWorkflow()
                {
                    WorkflowId = r.WorkDispatchId,
                    UserId = r.UserId,
                };
                var workDispatch = await workDispatchRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkDispatchId);
                if (workDispatch is null)
                {
                    return new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = "Không tìm thấy công văn!",
                        IsError = true,
                    };
                }

                if (r.IsSubmit)
                {
                    userWorkFlow.UserWorkflowType = UserWorkflowType.Submit;
                    workDispatch.WorkflowStatus = WorkflowStatusEnum.Submit;
                }
                else
                {
                    userWorkFlow.UserWorkflowType = UserWorkflowType.Signarture;
                    workDispatch.WorkflowStatus = WorkflowStatusEnum.Signarture;
                }
                await userWorkflowRepo.AddAsync(userWorkFlow);
                workDispatchRepo.Update(workDispatch);
                await _unitOfWork.CommitAsync();
                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = "Gửi công văn thành công!",
                    IsError = false,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
