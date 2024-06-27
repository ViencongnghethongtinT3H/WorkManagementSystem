namespace WorkManagementSystem.Features.File.CreateFolder;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

   public async Task<string> CreateFolders(FileManagement fileManagement, Request r)
    {
        var fileManagementRepository = _unitOfWork.GetRepository<FileManagement>();
        fileManagementRepository.Add(fileManagement);      
        await _unitOfWork.CommitAsync();
        return fileManagement.Id.ToString();
    }
}
