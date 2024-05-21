using WorkManagementSystem.Entities;

namespace WorkManagementSystem.Features.WorkItem.GetWorkItemByCondition;

public class Endpoint : Endpoint<Request, Response>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("WorkItem/get-work-item-by-condition");
    }

    public override async Task HandleAsync(Request query, CancellationToken c)
    {

        var queryModel = HttpContext.SafeGetListQuery<InputRequest, Response>(query.Query);

        var data = new Data(_unitOfWork);
        var a = await data.GetWorkItemByCondition(queryModel);
        await SendAsync(a);

      //   await SendNotFoundAsync();
        //var data = new Data(_unitOfWork);

        //var setting = await data.GetSettingByType(r.SettingType);

        //if (setting is null)
        //    await SendNotFoundAsync();
        //else
        //    await SendAsync(setting);
    }
}
