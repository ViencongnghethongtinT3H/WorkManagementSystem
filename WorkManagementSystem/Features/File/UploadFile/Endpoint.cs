namespace WorkManagementSystem.Features.File.UploadFile;

public class Endpoint : Endpoint<Request, ResultModel<List<Guid>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Post("/files/uploads-file");
        AllowFileUploads();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (Files.Count > 0)
        {
            var lst = new List<FileInfo>();

            var dirUpload = @"C:\Project\FileManagerService\Output\2023\file";
            foreach (var item in Files)
            {              
                if (!Directory.Exists(dirUpload))
                {
                    Directory.CreateDirectory(dirUpload);
                }
                var filePath = dirUpload + "\\" + item.FileName;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);
                }
                lst.Add(new FileInfo
                {
                    FileName = item.FileName,
                    FileUrl = filePath
                });
            }
            var data = new Data(_unitOfWork);
            var result = await data.AddFileAttachs(lst);
            await SendAsync(ResultModel<List<Guid>>.Create(result));
        }
    }
}
