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
            var userWorkFlow = new UserWorkflow() 
            {
                WorkflowId = r.WorkDispatchId,
                UserId = r.UserId,
            };

            try
            {
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
                    Data = false,
                    Status = 200,
                    ErrorMessage = "Gửi văn bản thành công!",
                    IsError = true,
                };
            }
            catch (Exception e)
            {
                throw;
            }


        }
    }
}
