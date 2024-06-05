namespace WorkManagementSystem.Features.File.UploadFile;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Guid>> AddFileAttachs(List<FileInfo> fileInfos)
    {
        var fileRepo = _unitOfWork.GetRepository<FileAttach>();
        var lst = new List<FileAttach>();
        foreach (var file in fileInfos)
        {
            lst.Add( new FileAttach
            {
                FileUrl = file.FileUrl,
                FileName = file.FileName,
                IssuesId = null,
                FileExtension = null,
            });
        }
        await fileRepo.AddRangeAsync(lst);
        await _unitOfWork.CommitAsync();


        return lst.Select(x => x.Id).ToList();
    }

   
}
