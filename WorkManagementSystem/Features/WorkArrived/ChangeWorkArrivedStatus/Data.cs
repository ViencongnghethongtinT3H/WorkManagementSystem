namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(ResultModel<bool>, string UserCompileId)> ChangeWorkArrivedStatus(Request r)
        {
            string UserCompileId = string.Empty;
            var workArrivedRepo = _unitOfWork.GetRepository<Entities.WorkArrived>();
            var userWorkflowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var workStepRepo = _unitOfWork.GetRepository<WorkflowStep>();
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
                    }, UserCompileId);
                }
                var workArrived = await workArrivedRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkArriveId);

                if (workArrived is null)
                {
                    return (new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = "Không tìm thấy công văn!",
                        IsError = true,
                    }, UserCompileId);
                }
                var userWorkflow = await  userWorkflowRepo.GetAll().FirstOrDefaultAsync(p => p.UserId == r.UserId && p.WorkflowId == r.WorkArriveId);
                if (userWorkflow is not null)
                {
                    userWorkflow.Note = r.Note;
                    userWorkflow.Updated = DateTime.Now;
                    workArrived.Updated = DateTime.Now;
                    if (r.ActionType == ActionType.Submited || r.ActionType == ActionType.Proccessing)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Proccesing;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.Proccesing;
                    }
                    else if (r.ActionType == ActionType.Canceled)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Cancel;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.Cancel;   // huỷ văn bản
                    }
                    else if (r.ActionType == ActionType.Return)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.ReceiveProccess;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.ReceiveProccess;  // trả lại văn bản
                    }
                    else if (r.ActionType == ActionType.Save)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.Complete;   // cong van duoc hoan thanh
                        var steps = workStepRepo.FindBy(p => p.WorkflowId == r.WorkArriveId);
                        if (steps.IsAny())
                        {
                            foreach (var item in steps)
                            {
                                item.Note = "Hoàn thành";
                                item.Step = StepEnum.Done;
                                item.Updated = DateTime.Now;
                                workStepRepo.Update(item);
                            }
                        }
                    }
                    else if (r.ActionType == ActionType.Swap)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.Waitting;  
                    }
                    // thay doi trang thai hanh dong xu ly cua van ban
                    userWorkflowRepo.Update(userWorkflow);
                    // thay đổi trạng thái của văn bản          
                    workArrivedRepo.Update(workArrived);
                }
                await _unitOfWork.CommitAsync();
                return (new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = "Cập nhật thành công!",
                    IsError = false,
                }, UserCompileId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
