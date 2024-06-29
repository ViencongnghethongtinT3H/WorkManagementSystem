using WorkManagementSystem.Features.ToImplementer;

namespace WorkManagementSystem.Features.WorkDispatch.AddUserToWorkDispatch;

// Chức năng chuyển công văn cho người/n-người theo dõi (bật popup lên cho chọn người theo dõi công văn và có thêm phần vai trò:  Người theo dõi, Người phối hợp/ xử lý)
public class Endpoint : Endpoint<Request, ResultModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventImplement _eventImplement;
    public Endpoint(IUnitOfWork unitOfWork, IEventImplement eventImplement)
    {
        _unitOfWork = unitOfWork;
        _eventImplement = eventImplement;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Post("/workDispatch/add-user-to-work-dispatch");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork, _eventImplement);
        var result = await data.AddUserToWorkDispatch(r);
        // Thêm phần lịch sử

        // Thêm phần notification

        await SendAsync(result);
    }
}
