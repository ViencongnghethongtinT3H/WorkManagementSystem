using WorkManagementSystem.Entities;

namespace WorkManagementSystem.Features.WorkDispatch.AddUserFollow
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> AddUserFollow(Request r)
        {
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
            var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var listUserFlow = new List<UserWorkflow>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();

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
                if (r.FollowUserType == FollowUserType.Follow)
                {
                    userWorkFlow.UserWorkflowType = UserWorkflowType.Followers;
                }
                else if (r.FollowUserType == FollowUserType.Combination)
                {
                    userWorkFlow.UserWorkflowType = UserWorkflowType.Combination;
                }
                else
                {
                    userWorkFlow.UserWorkflowType = UserWorkflowType.Implementer;
                }
                listUserFlow.Add(userWorkFlow);
            }
          

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
               
                if(listUserFlow is not null)
                {
                   await userWorkflowRepo.AddRangeAsync(listUserFlow);
                }
                await _unitOfWork.CommitAsync();
                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = "Thêm người theo dõi thành công!",
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
