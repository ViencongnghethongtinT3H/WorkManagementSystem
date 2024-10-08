
namespace WorkManagementSystem.Features.Folder.UpdateFolder;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<System.Guid>> UpdateFolder(Request r)
    {
        var fileManagementRepository = _unitOfWork.GetRepository<FileManagement>();
        var folder = await fileManagementRepository.GetAll().FirstOrDefaultAsync(x => x.Id == r.Id);
        if (folder == null)
        {
            return new ResultModel<System.Guid>(r.Id)
            {
                Data = r.Id,
                Status = 404,
                ErrorMessage = "Folder không tồn tại",
                IsError = true,
            };
        }
        var existingFolder = await fileManagementRepository.GetAll().AnyAsync(x => x.ParentId == folder.ParentId && x.Name == r.Name);
        if (existingFolder)
        {
            return new ResultModel<System.Guid>(r.Id)
            {
                Data = r.Id,
                Status = 409,
                ErrorMessage = "Tên folder này đã tồn tại",
                IsError = true,
            };
        }
        folder.Name = r.Name;
        fileManagementRepository.Update(folder);
        await _unitOfWork.CommitAsync();
        return new ResultModel<System.Guid>(r.Id)
        {
            Data = r.Id,
            Status = 200,
            ErrorMessage = "Cập nhật folder thành công",
            IsError = false,
        };

    }
}
