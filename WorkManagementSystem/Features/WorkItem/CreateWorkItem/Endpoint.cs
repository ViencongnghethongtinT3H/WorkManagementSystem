namespace WorkManagementSystem.Features.WorkItem.CreateWorkItem;
public class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkItem/create");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);

        Response.WorkId = await data.CreateWorkItems(Map.ToEntity(r));

        if (string.IsNullOrEmpty(Response.WorkId))
            ThrowError("Không thể thêm công văn");
        await SendAsync(Response);

    }
}
