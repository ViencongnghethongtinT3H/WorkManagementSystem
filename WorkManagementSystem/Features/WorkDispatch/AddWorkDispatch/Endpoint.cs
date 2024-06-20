namespace WorkManagementSystem.Features.WorkDispatch.AddWorkDispatch;

public class Endpoint : Endpoint<Request, ResultModel<Response>, Mapper>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/WorkDispatch/create");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = ResultModel<Response>.Create(new Response
        {
            WorkItemId = await data.CreateWorkDispatch(Map.ToEntity(r), r)
        });
        var name = await data.GetUserName(r);

        await new HistoryCommand
        {
            UserId = r.UserCompile,
            IssueId = new Guid(result.Data.WorkItemId),
            ActionContent = $"Tài khoản {name} đã tạo thêm một công văn"
        }.ExecuteAsync();

        if (string.IsNullOrEmpty(result.Data.WorkItemId))
            ThrowError("Không thể thêm công văn");
        await SendAsync(result);

    }
}
