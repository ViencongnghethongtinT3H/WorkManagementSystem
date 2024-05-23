namespace WorkManagementSystem.Features.History;

public class Endpoint : Endpoint<Request, ResultModel<List<Response>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("history/get-history-by-issuesId");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var histories = await data.GetHistoryByIssuesId(r);
        var result = ResultModel<List<Response>>.Create(histories);
        
        if (result is null)
            await SendNotFoundAsync();
        else
            await SendAsync(result);
    }
}