
namespace WorkManagementSystem.Features.Folder.GetListFolder;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Response>> GetFolder(Request r)
    {
        var result = new List<Response>();
        await GetFolderRecursively(r.ParentId, r.UserId, result);
        return result;
    }

    private async Task GetFolderRecursively(Guid? parentId, Guid userId, List<Response> result)
    {
        var fileManagement = _unitOfWork.GetRepository<FileManagement>().GetAll();
        var fileAttach = _unitOfWork.GetRepository<FileAttach>().GetAll();

        var folders = await fileManagement
            .Where(x => x.ParentId == parentId && x.UserId == userId)
            .Select(fm => new FolderModel
            {
                Id = fm.Id,
                Name = fm.Name,
                UserId = fm.UserId,
                ParentId = fm.ParentId,
                FileManagementType = fm.FileManagementType,
            }).ToListAsync();

        foreach (var folder in folders)
        {
            var fileAttaches = await fileAttach.Where(x => x.RefId == folder.Id).ToListAsync();
            var folderResponse = new Response
            {
                Folder = folder,
                Files = fileAttaches,
                Children = new List<Response>()
            };
            result.Add(folderResponse);

            // Đệ quy để lấy các thư mục con
            await GetFolderRecursively(folder.Id, userId, folderResponse.Children);
        }
    }
}
