namespace WorkManagementSystem.Features.File.CreateFolder;

public class Endpoint : Endpoint<Request, ResultModel<Response>, Mapper>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Post("/folders/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var data = new Data(_unitOfWork);
        var create = await data.CreateFolders(Map.ToEntity(req), req);
        var result = ResultModel<Response>.Create(new Response
        {
            Message = create
        });
        await SendAsync(result);

    }
}
