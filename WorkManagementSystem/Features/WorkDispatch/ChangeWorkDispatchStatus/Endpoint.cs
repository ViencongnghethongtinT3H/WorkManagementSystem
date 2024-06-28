namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus;

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
        Post("/workDispatch/change-work-dispatch-status");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        var result = await data.ChangeApproveWorkDispatch(r);


        // Thêm phần lịch sử

        // Thêm phần notification

        await SendAsync(result);
    }
}
