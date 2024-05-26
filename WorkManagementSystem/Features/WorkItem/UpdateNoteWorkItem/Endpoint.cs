namespace WorkManagementSystem.Features.WorkItem.UpdateNoteWorkItem;

public class Endpoint : Endpoint<Request, ResultModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkItem/update-implemeter-note");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.UpdateNoteWorkItem(r);
        await SendAsync(result);
    }
}
