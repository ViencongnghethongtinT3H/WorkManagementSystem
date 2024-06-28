namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;
// Thay đổi trạng thái của công văn đi (luồng trình/duyệt)  => thêm phần note
public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultModel<bool>> ChangeApproveWorkDispatch(Request r)
    {
        var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
        var userRepo = _unitOfWork.GetRepository<Entities.User>();
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
                
                workDispatch.WorkflowStatus = r.WorkflowStatus;
                workDispatchRepo.Update(workDispatch);
                await _unitOfWork.CommitAsync();
                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = "Duyệt công văn thành công!",
                    IsError = false,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


    }
}
