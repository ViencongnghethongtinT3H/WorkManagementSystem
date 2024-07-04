using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WorkManagementSystem.Features.WorkArrived.SaveWorkArrived
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> SaveWorkArrived(Request r)
        {
            var folderRepo = _unitOfWork.GetRepository<FileManagement>();
            var fileRepo = _unitOfWork.GetRepository<FileAttach>();
            var workItemRepo = _unitOfWork.GetRepository<Entities.WorkArrived>();
            var workItem = await workItemRepo.FindBy(p=>p.WorkItemNumber == r.WorkItemNumber).FirstOrDefaultAsync();
            if(workItem is not null)
            {
                var folder = new FileManagement()
                {
                    Created = DateTime.Now,
                    FileManagementType = FileManagementType.WorkItem,
                    Name = r.WorkItemNumber,
                    UserId = r.UserId,
                    ParentId = null,
                };
                await folderRepo.AddAsync(folder);
                var file = await fileRepo.FindBy(p => p.IssuesId == workItem.Id).FirstOrDefaultAsync();
                if(file is not null) 
                {
                    file.RefId = folder.Id;
                    file.Updated = DateTime.Now;
                    fileRepo.Update(file);
                }
                await _unitOfWork.CommitAsync();
                return new ResultModel<bool>(true)
                {
                    Data = true,
                    Status = 200,
                    ErrorMessage = string.Empty,
                    IsError = false,
                };
            }
            else
            {
                return new ResultModel<bool>(false)
                {
                    Data = false,
                    Status = 200,
                    ErrorMessage = "Không tìm thấy công văn",
                    IsError = true,
                };
            }


        }
    }
}
