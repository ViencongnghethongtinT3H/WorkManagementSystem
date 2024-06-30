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
            var lst = new List<UserWorkflow>();
            foreach (var item in r.UserProccess)
            {
                var user = new UserWorkflow
                {
                    UserId = item.UserIds,
                    WorkflowId = r.WorkflowId,
                    UserWorkflowType = item.UserWorkflowType,   // add theo vai trò
                    UserWorkflowStatus = UserWorkflowStatusEnum.Waitting,    // mặc định chuyển người xử lý thì gán mặc định là 1,
                    Note = item.Note
                };
                lst.Add(user);
            }
            await userWorkflowRepo.AddRangeAsync(lst);
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

