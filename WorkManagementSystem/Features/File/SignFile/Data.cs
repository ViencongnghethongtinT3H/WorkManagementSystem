namespace WorkManagementSystem.Features.File.SignFile;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> SignFile(Request r)
    {
        var fileRepo = _unitOfWork.GetRepository<FileAttach>();
        var lst = new List<FileAttach>();
        var fileInfo = await fileRepo.GetAll().FirstOrDefaultAsync(x => x.Id == r.FileId);
        if (fileInfo is not null)
        {
           var output =  await new SignFileCommand
            {
                FileName = fileInfo.FileName,
                FileUrl = fileInfo.FileUrl,
            }.ExecuteAsync();
            var file = new FileAttach
            {
                IssuesId = fileInfo.IssuesId,
                Status = StatusEnum.Active,

            };

            return output;
        }
        
        return string.Empty;
    }


}
