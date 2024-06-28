namespace WorkManagementSystem.Features.Folder.CreateFolder;

public class Endpoint : Endpoint<Request, ResultModel<bool>, Mapper>
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
        var result = await data.CreateFolders(Map.ToEntity(req), req);

        await SendAsync(result);

    }
}
