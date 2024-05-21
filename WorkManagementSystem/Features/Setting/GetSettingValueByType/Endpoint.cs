using WorkManagementSystem.Features.Department.GetListDepartment;

namespace WorkManagementSystem.Features.Setting.GetSettingValueByType;

public class Endpoint : Endpoint<Request, ListResultSelectModel<SettingModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("setting/get-setting-by-type/{settingType}");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);

        var setting = await data.GetSettingByType(r.SettingType);

        var result = ListResultSelectModel<SettingModel>.Create(setting);

        if (setting is null)
            await SendNotFoundAsync();
        else
            await SendAsync(result);
    }
}
