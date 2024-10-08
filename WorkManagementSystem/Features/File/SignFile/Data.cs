using System.Net;

namespace WorkManagementSystem.Features.File.SignFile;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> SignFile(Request r)
    {
        var fileRepo = _unitOfWork.GetRepository<FileAttach>();
        var lst = new List<FileAttach>();
        string output = string.Empty;
        if (r.TypeFile == 1)
        {
            output = await new SignFileCommand
            {
                FileName = "",
                FileUrl = "",
                TypeFile =r.TypeFile,
            }.ExecuteAsync();
        }
        else
        {
            var fileInfo = await fileRepo.GetAll().FirstOrDefaultAsync(x => x.Id == r.FileId);
            if (fileInfo is not null)
            {
                output = await new SignFileCommand
                {
                    FileName = fileInfo.FileName,
                    FileUrl = fileInfo.FileUrl,
                    TypeFile = r.TypeFile,

                }.ExecuteAsync();
                var file = new FileAttach
                {
                    IssuesId = fileInfo.IssuesId,
                    Status = StatusEnum.Active,
                    FileUrl = @"C:\Project\FileManagerService\Output\2023\file\signature\" + GetFileNameFromUrl(output),
                    FileExtension = "pdf",
                    FileName = GetFileNameFromUrl(output)

                };
                await fileRepo.AddAsync(file);
                await _unitOfWork.CommitAsync(); 
            }
        }
        return output;
    }
    public static string GetFileNameFromUrl(string url)
    {
        // Sử dụng Uri để phân tích URL
        Uri uri = new Uri(url);

        // Lấy đường dẫn từ URL
        string path = uri.AbsolutePath;

        // Sử dụng Path.GetFileName để lấy tên tệp từ đường dẫn
        string fileName = Path.GetFileName(path);

        // Giải mã URL để lấy tên tệp gốc
        string decodedFileName = WebUtility.UrlDecode(fileName);

        return decodedFileName;
    }

}
