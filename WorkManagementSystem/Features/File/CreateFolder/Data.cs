namespace WorkManagementSystem.Features.File.CreateFolder;

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
