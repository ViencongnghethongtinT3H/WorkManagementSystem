

namespace WorkManagementSystem.Features.WorkArrived.GetListWorkArrivedByCondition;

public class Endpoint : Endpoint<Request, ListResultModel<Response>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("/workArrived/get-by-condition");
    }

    public override async Task HandleAsync(Request query, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        await SendAsync(await data.GetWorkDispatchByCondition(query));
    }
}
