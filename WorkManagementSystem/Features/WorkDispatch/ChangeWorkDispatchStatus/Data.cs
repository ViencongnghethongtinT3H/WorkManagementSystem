namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;
// Thay đổi trạng thái của công văn đi 
public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultModel<bool>> ChangeApproveWorkDispatch(Request r)
    {
        var userNotifications = new List<Guid>();
        var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
        var userRepo = _unitOfWork.GetRepository<Entities.User>();
        var implementRepository = _unitOfWork.GetRepository<Implementer>();

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
            var workDispatch = await workDispatchRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkFlowId);
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

            // thay đổi trạng thái của văn bản 
            workDispatch.WorkflowStatus = r.WorkflowStatus;

            workDispatchRepo.Update(workDispatch);
         
            
            var userWorkflow = await userWorkflowRepo.GetAll().FirstOrDefaultAsync(p => p.UserId == r.UserId);           
            if (userWorkflow is not null)
            {
                userWorkflow.Note = r.Note;
                userWorkflow.Updated = DateTime.Now;

                if (r.WorkflowStatus == WorkflowStatusEnum.Submited || r.WorkflowStatus == WorkflowStatusEnum.Signartured)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
                }
                else if (r.WorkflowStatus == WorkflowStatusEnum.ReceiveProccess)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.ReceiveProccess;
                }
                else if (r.WorkflowStatus == WorkflowStatusEnum.Cancel)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Cancel;
                }
                else if (r.WorkflowStatus == WorkflowStatusEnum.Waitting)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Proccesing;
                }

                userWorkflowRepo.Update(userWorkflow);
            }

            await _unitOfWork.CommitAsync();
            return new ResultModel<bool>(true)
            {
                Data = true,
                Status = 200,
                ErrorMessage = "Cập nhật thành công!",
                IsError = false,
            };
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
