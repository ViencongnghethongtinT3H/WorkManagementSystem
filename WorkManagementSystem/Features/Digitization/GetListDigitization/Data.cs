namespace WorkManagementSystem.Features.Digitization.GetListDigitization;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Response>> GetList(Request request)
    {
        return await _unitOfWork.GetRepository<Entities.Digitization>().FindBy(x => x.OcrType == request.OcrType).Select(x => new Response
        {
            OcrType = x.OcrType,
            Data = x.Data,
            Url = x.Url,
        }).ToListAsync();

    }
}
