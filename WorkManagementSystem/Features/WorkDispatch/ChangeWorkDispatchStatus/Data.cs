namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;
// Thay đổi trạng thái của công văn đi 
public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<(ResultModel<bool>, string UserCompileId)> ChangeApproveWorkDispatch(Request r)
    {
        var userNotifications = new List<Guid>();
        var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
        var userRepo = _unitOfWork.GetRepository<Entities.User>();

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
                },string.Empty);
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
                },string.Empty);
            }
            var userCompileId = workDispatch.UserCompile.Value;
            var userWorkflow = await userWorkflowRepo.GetAll().FirstOrDefaultAsync(p => p.UserId == r.UserId);           
            if (userWorkflow is not null)
            {
                userWorkflow.Note = r.Note;
                userWorkflow.Updated = DateTime.Now;

                if (r.ActionType == ActionType.Submited || r.ActionType == ActionType.Signatured)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
                    workDispatch.WorkflowStatus = WorkflowStatusEnum.Proccesing;
                }
                else if (r.ActionType == ActionType.Canceled)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Cancel;
                    workDispatch.WorkflowStatus = WorkflowStatusEnum.Cancel;   // huỷ văn bản
                }
                else if (r.ActionType == ActionType.Return)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.ReceiveProccess;
                    workDispatch.WorkflowStatus = WorkflowStatusEnum.ReceiveProccess;  // trả lại văn bản
                }
                else if (r.ActionType == ActionType.Proccessing)
                {
                    userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Proccesing;
                    workDispatch.WorkflowStatus = WorkflowStatusEnum.Proccesing;
                }

                userWorkflowRepo.Update(userWorkflow);
                // thay đổi trạng thái của văn bản          
                workDispatchRepo.Update(workDispatch);
            }
            await _unitOfWork.CommitAsync();
            return (new ResultModel<bool>(true)
            {
                Data = true,
                Status = 200,
                ErrorMessage = "Cập nhật thành công!",
                IsError = false,
            }, userCompileId.ToString());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
