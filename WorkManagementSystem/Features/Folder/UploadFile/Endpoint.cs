using System.Net.Http.Headers;
using System.Text;

namespace WorkManagementSystem.Features.Folder.UploadFile;


public class Endpoint : Endpoint<Request, Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public Endpoint(IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _config = config;
    }
    public override void Configure()
    {
        Post("/folders/uploads-file");
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
            var result = await data.AddFileAttachs(lst, req);
            var kq = new Response();
            kq.ids = result;
            await SendAsync(kq);
        }
    }

}
