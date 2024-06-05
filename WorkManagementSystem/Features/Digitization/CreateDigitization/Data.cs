namespace WorkManagementSystem.Features.Digitization.CreateDigitization
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;

        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddData(Request request)
        {
            var fileRepo = _unitOfWork.GetRepository<Entities.Digitization>();
            var data = new Entities.Digitization
            {
                OcrType = request.OcrType,
                Data = request.Data,
                Url = request.Url
            };
            await fileRepo.AddAsync(data);
            await _unitOfWork.CommitAsync();    
            return true;
        }
    }
}
