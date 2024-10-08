
namespace WorkManagementSystem.Features.Folder.DeleteFolder;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<System.Guid>> DeleteFolder(Request r)
    {
        var fileManagementRepository = _unitOfWork.GetRepository<FileManagement>();
        var folders = await fileManagementRepository.FindBy(x => x.Id == r.Id).ToListAsync();
        if (folders == null || folders.Count == 0)
        {
            return new ResultModel<System.Guid>(r.Id)
            {
                Data = r.Id,
                Status = 404,
                ErrorMessage = "Folder không tồn tại",
                IsError = true,
            };
        }
        fileManagementRepository.HardDeletes(folders);
        await _unitOfWork.CommitAsync();
        return new ResultModel<System.Guid>(r.Id)
        {
            Data = r.Id,
            Status = 200,
            ErrorMessage = "Xóa folder thành công",
            IsError = false,
        };

    }
}
