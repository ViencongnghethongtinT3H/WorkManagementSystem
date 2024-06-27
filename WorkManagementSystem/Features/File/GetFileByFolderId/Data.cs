namespace WorkManagementSystem.Features.File.GetFileByFolderId;


public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Response>> GetList(Request r)
        {
            var fileManagement = await _unitOfWork.GetRepository<Entities.FileAttach>().GetAll().Where(x => x.RefId == r.FolderId).ToListAsync();
            return fileManagement.Select(fm => new Response
            {
                Id = fm.Id,
                FileName = fm.FileName,
                FileUrl = fm.FileUrl,
                IssuesId=fm.IssuesId,
                RefId=fm.RefId,
                FileExtension=fm.FileExtension
            }).ToList();
        }
}
