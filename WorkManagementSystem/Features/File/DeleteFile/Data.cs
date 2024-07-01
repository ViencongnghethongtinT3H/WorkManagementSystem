
namespace WorkManagementSystem.Features.File.DeleteFile;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<bool>> DeleteFile(Request r)
    {
        var FileAttachRepository = _unitOfWork.GetRepository<FileAttach>();
        var file = await FileAttachRepository.FindBy(x => x.Id == r.Id).ToListAsync();
        if (file == null || file.Count == 0)
        {
            return new ResultModel<bool>(false)
            {
                Data = false,
                Status = 404,
                ErrorMessage = "File không tồn tại",
                IsError = true,
            };
        }
        FileAttachRepository.HardDeletes(file);
        await _unitOfWork.CommitAsync();
        return new ResultModel<bool>(true)
        {
            Data = true,
            Status = 200,
            ErrorMessage = "Xóa file thành công",
            IsError = false,
        };

    }
}
