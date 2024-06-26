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
                var workArrived = await workArrivedRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkArriveId);
                if (workArrived is null)
                {
                    return new ResultModel<bool>(false)
                    {
                        Data = false,
                        Status = 200,
                        ErrorMessage = "Không tìm thấy công văn đến!",
                        IsError = true,
                    };
                }
                workArrived.WorkArrivedStatus = r.WorkArrivedStatus;
                workArrivedRepo.Update(workArrived);
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
}
