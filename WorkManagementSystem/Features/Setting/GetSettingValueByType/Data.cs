namespace WorkManagementSystem.Features.Setting.GetSettingValue;
public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<SettingModel>> GetSettingByType(SettingEnum settingType) 
    {
        var setting = _unitOfWork.GetRepository<Entities.Setting>();
        return await setting.FindBy(x => x.Type == settingType).Select(x => new SettingModel
        {
            Key = x.Key,
            Value = x.Value
        }).ToListAsync();
    
    }
}
