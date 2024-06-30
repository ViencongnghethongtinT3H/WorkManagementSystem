
namespace WorkManagementSystem.Features.Folder.ShareFile;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<FileManagement>> ShareFile(Request r)
    {

        try
        {
            var fileManagementRepo = _unitOfWork.GetRepository<FileManagement>();
            var fileAttachRepo = _unitOfWork.GetRepository<FileAttach>();
            var fileManagement = await fileManagementRepo.GetAll()
                     .FirstOrDefaultAsync(x => x.Id == r.FolderId);
            var file = await fileAttachRepo.GetAll()
                     .FirstOrDefaultAsync(x => x.Id == r.FileId);
            var newFileAttach = new FileAttach
            {
                IssuesId = file.IssuesId,
                FileName = file.FileName,
                FileUrl = file.FileUrl,
                RefId = fileManagement.Id,
                FileExtension = file.FileExtension
            };
            fileAttachRepo.Add(newFileAttach);
            await _unitOfWork.CommitAsync();

            return new ResultModel<FileManagement>(fileManagement)
            {
                Data = fileManagement,
                Status = 200,
                ErrorMessage = "Chia sẻ file thành công",
                IsError = false,
            };
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);

        }


    }
}
