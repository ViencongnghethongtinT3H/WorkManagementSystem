namespace WorkManagementSystem.Features.Digitization.CreateDigitization;

public class Endpoint : Endpoint<Request, ResultModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Post("/digitization/add-data");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var data = new Data(_unitOfWork);
        var result = await data.AddData(req);
        await SendAsync(ResultModel<bool>.Create(result));

    }

}