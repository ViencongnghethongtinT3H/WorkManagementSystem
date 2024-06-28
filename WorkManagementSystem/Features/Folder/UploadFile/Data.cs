namespace WorkManagementSystem.Features.Folder.UploadFile;


public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<bool>> AddFileAttachs(List<FileInfo> fileInfos, Request req)
    {
        var fileRepo = _unitOfWork.GetRepository<FileAttach>();
        var lst = new List<FileAttach>();
        foreach (var file in fileInfos)
        {
            lst.Add(new FileAttach
            {
                FileUrl = file.FileUrl,
                FileName = file.FileName,
                IssuesId = null,
                FileExtension = null,
                RefId = req.RefId
            });
        }
        await fileRepo.AddRangeAsync(lst);
        await _unitOfWork.CommitAsync();


         return new ResultModel<bool>(true)
        {
            Data = true,
            Status = 200,
            ErrorMessage = "Upload file thành công",
            IsError = false,
        };
    }


}
