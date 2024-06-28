using WorkManagementSystem.Features.ToImplementer;
namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch
{
    // Chuyển người xử lý bước tiếp theo  => thêm phần note
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventImplement _eventImplement;
        public Data(IUnitOfWork unitOfWork, IEventImplement eventImplement)
        {
            _unitOfWork = unitOfWork;
            _eventImplement = eventImplement;
        }
        public async Task<(ResultModel<bool>, List<Guid> UserReceiveNotification)> AddUserToWorkDispatch(Request r)
        {
            var userNotifications = new List<Guid>();
            var implementRepository = _unitOfWork.GetRepository<Implementer>();
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
            var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var listUserFlow = new List<UserWorkflow>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();

            var workDispatch = await workDispatchRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkflowId);

            if (workDispatch is null)
            {
                return (new ResultModel<bool>(false)
                {
                    Data = false,
                    Status = 200,
                    ErrorMessage = "Không tìm thấy công văn!",
                    IsError = true,
                }, userNotifications);
            }


            foreach (var item in r.UserIds)
            {
                var user = await userRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == item);
                if (user is null)
                {
                    return (new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = $"Không tìm thông tin người dùng theo Id = ${item}!",
                        IsError = true,
                    }, userNotifications);
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
            var implementOld = await implementRepository.FindBy(x => x.IssuesId == r.WorkflowId).ToListAsync();
            if (implementOld is not null)
            implementRepository.HardDeletes(implementOld);
            var implements = _eventImplement.ToImplementer(r.RequestImplementer, r.WorkflowId);
            await implementRepository.AddRangeAsync(implements);
            userNotifications = implements.Select(x => x.UserReceiveId).ToList();

            await _unitOfWork.CommitAsync();
            return (new ResultModel<bool>(true)
            {
                Data = true,
                Status = 200,
                ErrorMessage = "Thêm thành công!",
                IsError = false,
            }, userNotifications);
        }
        public async Task<string> GetUserName(Guid UserId)
        {
            var user = await _unitOfWork.GetRepository<Entities.User>().GetAsync(UserId);
            if (user is not null)
            {
                return user.Name;
            }
            return string.Empty;

        }

    }
}

