namespace WorkManagementSystem.Features.User.GetListUserForSelect;

public class Endpoint : Endpoint<Request, ResultModel<List<UserModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/user/get-users");
    }

    public override async Task HandleAsync(Request request, CancellationToken c)
    {
        var data = new Data(_unitOfWork);
        Response = await data.GetUserInfo(request);
        await SendAsync(Response);
    }
}
