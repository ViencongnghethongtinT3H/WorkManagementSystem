namespace WorkManagementSystem.Features.Setting.GetSettingValue;

public class Endpoint : Endpoint<Request, List<SettingModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Endpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("/setting/get-setting-by-ty/{settingType}");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var data = new Data(_unitOfWork);

        var setting = await data.GetSettingByType(r.SettingType);

        if (setting is null)
            await SendNotFoundAsync();
        else
            await SendAsync(setting);
    }
}
