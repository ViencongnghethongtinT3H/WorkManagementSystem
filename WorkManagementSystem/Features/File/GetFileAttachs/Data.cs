namespace WorkManagementSystem.Features.File.GetFileAttachs;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FileViewModel>> GetFileAttachs(Request r)
    {
        var fileRepo = await _unitOfWork.GetRepository<FileAttach>().GetAll().Where(x => x.IssuesId == r.IssuesId).Select (x => new FileViewModel
        {
            FileName = x.FileName,
            FileUrl = $"https://file-manager.digins.vn/Output/2023/file/{x.FileName}",
            Id = x.Id
        }).ToListAsync();
       return fileRepo;
    }
}
