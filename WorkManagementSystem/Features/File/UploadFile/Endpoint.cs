using System.Net.Http.Headers;
using System.Text;

namespace WorkManagementSystem.Features.File.UploadFile;

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
            await data.AddFileAttachs(lst);

            var kq = new Response();
            if (req.OcrType != OcrType.Other)
            {
                kq.Data = await OcrFile(Files.FirstOrDefault(), req.OcrType); ;
                kq.FileUrl = $"https://file-manager.digins.vn/Output/2023/file/{lst.FirstOrDefault().FileName}";
            }            

            await SendAsync(kq);


        }
    }
    public async Task<string> OcrFile(IFormFile file, OcrType ocrType)
    {
        string username = _config.GetValue("Computervision:ApiKey", "");
        string password = _config.GetValue("Computervision:ApiSecret", "");
        string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

        _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", svcCredentials);
        using (var formData = new MultipartFormDataContent())
        {
            var ms = new StreamContent(file.OpenReadStream());
            ms.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            var name = "img";

            formData.Add(ms, name, file.FileName);
            var url = ReturnOcrUrl(ocrType);
            var response = await _httpClient.PostAsync(url, formData);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
    private string ReturnOcrUrl(OcrType ocrType)
    {
        string url = string.Empty;
        switch (ocrType)
        {
            case OcrType.CCCD:
                url = _config.GetValue("UrlOcr:CCCD", "");
                break;
            case OcrType.VehicleRegistrations:
                url = _config.GetValue("UrlOcr:VehicleRegistrations", "");
                break;
            case OcrType.VehicleInspection:
                url = _config.GetValue("UrlOcr:VehicleInspection", "");
                break;
            case OcrType.Vehicle:
                url = _config.GetValue("UrlOcr:Vehicle", "");
                break;
            case OcrType.VehiclePlate:
                url = _config.GetValue("UrlOcr:VehiclePlate", "");
                break;
            case OcrType.BirthCertificate:
                url = _config.GetValue("UrlOcr:BirthCertificate", "");
                break;
        }

        url += "?format_type=file&get_thumb=false";
        return url;
    }
}
