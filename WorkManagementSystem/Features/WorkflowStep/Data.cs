namespace WorkManagementSystem.Features.WorkStep
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> CreateWorkStep(WorkflowStep model, Request r)
        {   
            var workStepRepo = _unitOfWork.GetRepository<WorkflowStep>();
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();

            try
            {
                var workDispatch = await workDispatchRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkflowId);
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
                var user = await userRepo.GetAll().FirstOrDefaultAsync(p => p.Id == r.UserConfirm);
                if (user is null)
                {
                    return new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = "Người dùng không tồn tại!",
                        IsError = true,
                    };
                }
                model.Step = StepEnum.ManagerApprove;
                workStepRepo.Add(model);
                await _unitOfWork.CommitAsync();
                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = "",
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
