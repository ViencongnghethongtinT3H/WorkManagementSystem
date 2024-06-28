namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch
{
    // Chuyển người xử lý bước tiếp theo  => thêm phần note
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> AddUserToWorkDispatch(Request r)
        {
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
            var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var listUserFlow = new List<UserWorkflow>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();
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


            foreach (var item in r.UserIds)
            {
                var user = await userRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == item);
                if (user is null)
                {
                    return new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = $"Không tìm thông tin người dùng theo Id = ${item}!",
                        IsError = true,
                    };
                }
                var userWorkFlow = new UserWorkflow();
                userWorkFlow.UserId = item;
                userWorkFlow.WorkflowId = r.WorkflowId;
                userWorkFlow.UserWorkflowType = r.UserWorkflowType;
                listUserFlow.Add(userWorkFlow);
            }
            if (listUserFlow is not null)
            {
                await userWorkflowRepo.AddRangeAsync(listUserFlow);
            }
            await _unitOfWork.CommitAsync();
            return new ResultModel<bool>(true)
            {
                Data = true,
                Status = 200,
                ErrorMessage = "Thêm thành công!",
                IsError = false,
            };
        }


    }
}

