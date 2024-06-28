
namespace WorkManagementSystem.Features.File.GetFolders;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Response>> GetFolder(Request r)
    {
        var fileManagement = _unitOfWork.GetRepository<FileManagement>().GetAll();
        var fileAttach = _unitOfWork.GetRepository<FileAttach>().GetAll();

        var folders = await fileManagement.Where(x => x.ParentId == r.ParentId && x.UserId == r.UserId).Select(fm => new FolderModel
        {
            Id = fm.Id,
            Name = fm.Name,
            UserId =fm.UserId,
            ParentId = fm.ParentId,
            FileManagementType = fm.FileManagementType,
        }).ToListAsync();

        var result = new List<Response>();

        foreach (var folder in folders)
        {
            var fileAttaches = await fileAttach.Where(x => x.RefId == folder.Id).ToListAsync();
            var folderDto = new Response
            {
                Folders = folder,
                Files = fileAttaches
            };
            result.Add(folderDto);
        }
        return result;
    }
}
