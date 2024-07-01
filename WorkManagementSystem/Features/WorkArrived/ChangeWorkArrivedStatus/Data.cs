namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> ChangeWorkArrivedStatus(Request r)
        {
            var workArrivedRepo = _unitOfWork.GetRepository<Entities.WorkArrived>();
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
                    });
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
                    });
                }
                var userWorkflow = await userWorkflowRepo.GetAll().FirstOrDefaultAsync(p => p.UserId == r.UserId);
                if (userWorkflow is not null)
                {
                    userWorkflow.Note = r.Note;
                    userWorkflow.Updated = DateTime.Now;

                    if (r.ActionType == ActionType.Submited)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
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
                    else if (r.ActionType == ActionType.Proccessing)
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Proccesing;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.Proccesing;
                    }
                    else
                    {
                        userWorkflow.UserWorkflowStatus = UserWorkflowStatusEnum.Done;
                        workArrived.WorkArrivedStatus = WorkArrivedStatus.Complete;   // cong van duoc hoan thanh
                    }
                    // thay doi trang thai hanh dong xu ly cua van ban
                    userWorkflowRepo.Update(userWorkflow);
                    // thay đổi trạng thái của văn bản          
                    workArrivedRepo.Update(workArrived);
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
    }
}
