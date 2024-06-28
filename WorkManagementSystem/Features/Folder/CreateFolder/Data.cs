namespace WorkManagementSystem.Features.Folder.CreateFolder;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<bool>> CreateFolders(FileManagement fileManagement, Request r)
    {
        var fileManagementRepository = _unitOfWork.GetRepository<FileManagement>();
        var existingFolder = await fileManagementRepository.GetAll().AnyAsync(x => x.ParentId == r.ParentId && x.Name == r.Name);
        if (existingFolder)
        {
            return new ResultModel<bool>(false)
            {
                Data = false,
                Status = 409,
                ErrorMessage = "Tên folder này đã tồn tại",
                IsError = true,
            };
        }

        fileManagementRepository.Add(fileManagement);
        await _unitOfWork.CommitAsync();

        return new ResultModel<bool>(true)
        {
            Data = true,
            Status = 200,
            ErrorMessage = "Tạo mới folder thành công",
            IsError = false,
        };
    }
}
