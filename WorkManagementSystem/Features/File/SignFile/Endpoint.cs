using System.Net.Http.Headers;
using System.Text;
using WorkManagementSystem.Shared.Dtos;

namespace WorkManagementSystem.Features.File.SignFile;

public class Endpoint : Endpoint<Request, ResultModel<Response>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Post("/files/sign-file");
        AllowFileUploads();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var data = new Data(_unitOfWork);
        var result = await data.SignFile(req);
        await SendAsync(ResultModel<Response>.Create( new Response
        {
            Output = result,
        }));       
    }

}
