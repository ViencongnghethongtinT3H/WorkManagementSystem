using WorkManagementSystem.Shared.Dtos;

namespace WorkManagementSystem.Features.WorkItem.ActiveWorkItem;

public class Endpoint : Endpoint<Request, ResultModel<Response>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkItem/active-workItem");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = await data.ActiveWorkItem(r)
        });

        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể thêm công văn");

        await SendAsync(result);

    }
}
