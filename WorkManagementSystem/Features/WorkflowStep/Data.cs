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
            var workArrivedRepo = _unitOfWork.GetRepository<Entities.WorkArrived>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();

            try
            {
                var workArrived = await workArrivedRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkflowId);
                if (workArrived is null)
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
                if (r.Id != null)
                {

                    model.Id = r.Id.Value;                  
                    workStepRepo.Update(model);                  
                    
                }
                else
                {
                    workStepRepo.Add(model);
                }
                await _unitOfWork.CommitAsync();
                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = string.Empty,
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
