namespace WorkManagementSystem.Features.WorkDispatch.ForwardWorkDispatch
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> ForwardWorkDispatch(Request r)
        {
            var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();

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
                var userWorkf = new UserWorkflow();
                userWorkf.WorkflowId = workDispatch.Id;
                userWorkf.UserId = r.UserId;    
                userWorkf.UserWorkflowType = UserWorkflowType.Forward;
                await userWorkflowRepo.AddAsync(userWorkf);
                await _unitOfWork.CommitAsync();


                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = "Chuyển công văn thành công!",
                    IsError = false,
                };
            }
            catch (Exception e)
            {
                throw;
            }


        }
    }
}
