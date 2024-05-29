using WorkManagementSystem.Features.File.UploadFile;

namespace WorkManagementSystem.Features.File.GetFileAttachs;

public class Endpoint : Endpoint<Request, ResultModel<List<FileViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Get("/files/get-files");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {        
            var data = new Data(_unitOfWork);
            var result = await data.GetFileAttachs(req);
            await SendAsync(ResultModel<List<FileViewModel>>.Create(result));        
    }
}
