using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;
// Thay đổi trạng thái của công văn đi (luồng trình/duyệt)  => thêm phần note
public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventImplement _eventImplement;
    public Data(IUnitOfWork unitOfWork, IEventImplement eventImplement)
    {
        _unitOfWork = unitOfWork;
        _eventImplement = eventImplement;
    }
    public async Task<(ResultModel<bool>, List<Guid> Notifications)> ChangeApproveWorkDispatch(Request r)
    {
        var userNotifications = new List<Guid>();
        var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var userWorkflowRepo = _unitOfWork.GetRepository<Entities.UserWorkflow>();
        var userRepo = _unitOfWork.GetRepository<Entities.User>();
        var implementRepository = _unitOfWork.GetRepository<Implementer>();

        try
        {
            var user = await userRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.UserId);
            if (user is null)
            {
                return (new ResultModel<bool>(false)
                {
                    Data = false,
                    Status = 200,
                    ErrorMessage = "Không tìm thông tin người dùng!",
                    IsError = true,
                },userNotifications);
            }
            var workDispatch = await workDispatchRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkFlowId);
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

            workDispatch.WorkflowStatus = r.WorkflowStatus;
            workDispatchRepo.Update(workDispatch);

            var implementOld = await implementRepository.FindBy(x => x.IssuesId == r.WorkFlowId).ToListAsync();
            if (implementOld is not null)
                implementRepository.HardDeletes(implementOld);
            var implements = _eventImplement.ToImplementer(r.RequestImplementer, r.WorkFlowId);
            await implementRepository.AddRangeAsync(implements);
            userNotifications = implements.Select(x => x.UserReceiveId).ToList();

            var userWorkflow = new UserWorkflow()
            {
                Created = DateTime.Now,
                UserWorkflowType = r.UserWorkflowType,
                Note = r.Note,
                WorkflowId = workDispatch.Id,

            };

            userWorkflowRepo.Add(userWorkflow);
            await _unitOfWork.CommitAsync();
            return (new ResultModel<bool>(true)
            {
                Data = true,
                Status = 200,
                ErrorMessage = "Cập nhật thành công!",
                IsError = false,
            }, userNotifications);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> GetUserName(Request r)
    {
        var user = await _unitOfWork.GetRepository<Entities.User>().GetAsync(r.UserId);
        if (user is not null)
        {
            return user.Name;
        }
        return string.Empty;

    }
}
