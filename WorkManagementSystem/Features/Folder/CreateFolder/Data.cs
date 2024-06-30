namespace WorkManagementSystem.Features.Folder.CreateFolder;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<FileManagement>> CreateFolders(FileManagement fileManagement, Request r)
    {
        var fileManagementRepository = _unitOfWork.GetRepository<FileManagement>();
        var existingFolder = await fileManagementRepository.GetAll().AnyAsync(x => x.ParentId == r.ParentId && x.Name == r.Name);
        if (existingFolder)
        {
            return new ResultModel<FileManagement>(null)
            {
                Data = null,
                Status = 409,
                ErrorMessage = "Tên folder này đã tồn tại",
                IsError = true,
            };
        }

        fileManagementRepository.Add(fileManagement);
        await _unitOfWork.CommitAsync();

        return new ResultModel<FileManagement>(fileManagement)
        {
            Data = fileManagement,
            Status = 200,
            ErrorMessage = "Tạo mới folder thành công",
            IsError = false,
        };
    }
}
