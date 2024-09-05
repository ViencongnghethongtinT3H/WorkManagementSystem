using System.Globalization;
using System.Security.Cryptography;
using WorkManagementSystem.Entities;
using static iTextSharp.text.pdf.AcroFields;
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
            var userWorkFlowRepo = _unitOfWork.GetRepository<UserWorkflow>();
            var fileManagerRepo = _unitOfWork.GetRepository<FileManagement>();

            var workItem = await workItemRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Id == r.WorkArriveId);
            if (workItem is not null)
            {
                int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
                workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
                workItem.Updated = DateTime.Now;
                workItemRepo.Update(workItem);
                // lấy ra toàn bộ các user đang theo dõi công văn 
                var userWorks = await userWorkFlowRepo.GetAll().AsNoTracking().Where(p => p.WorkflowId == r.WorkArriveId).ToListAsync();
                foreach (var userWork in userWorks)
                {
                    // tìm folder "công văn"
                    var parnetFolder = await fileManagerRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userWork.UserId && p.Name == "Công văn đến");
                    // kiem tra xem co folder dc tao ra tu WorkItemNumber hay chua
                    var folder = await fileManagerRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.Name == workItem.WorkItemNumber);
                    if (folder is null)
                    {
                        folder = new FileManagement()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.Now,
                            FileManagementType = FileManagementType.WorkItem,
                            Name = workItem.WorkItemNumber,
                            UserId = userWork.UserId,
                            ParentId = parnetFolder is not null ? parnetFolder.Id : null,
                        };
                        await fileManagerRepo.AddAsync(folder);
                    }
                    var file = await fileRepo.GetAll().AsNoTracking().FirstOrDefaultAsync(p => p.IssuesId == userWork.WorkflowId);
                    if (file is not null)
                    {
                        if (file.RefId == new Guid())
                        {
                            file.IssuesId = userWork.WorkflowId;
                            file.Updated = DateTime.Now;
                            file.RefId = folder.Id;
                            fileRepo.Update(file);
                            await _unitOfWork.CommitAsync();
                        }
                        else
                        {
                            file = new FileAttach()
                            { RefId = folder.Id, Created = DateTime.Now, IssuesId = userWork.WorkflowId, FileExtension = file.FileExtension, FileName = file.FileName, FileUrl = file.FileUrl };
                            await fileRepo.AddAsync(file);
                        }

                    }


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
