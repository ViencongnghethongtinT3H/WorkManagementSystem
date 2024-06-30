using System.Globalization;
using System.Security.Cryptography;

namespace WorkManagementSystem.Features.WorkArrived.AddWorkArrived
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> CreateWorkArrived(Entities.WorkArrived workItem, Request r)
        {
            var workArrivedRepository = _unitOfWork.GetRepository<Entities.WorkArrived>();      
            int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
            workItem.WorkItemNumber = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
            workItem.WorkArrivedStatus = WorkArrivedStatus.Waitting;
            try
            {
                if(r.Id != null)
                {
                    var workArrive = await workArrivedRepository.FindBy(p=>p.Id == r.Id).FirstOrDefaultAsync();
                    if(workArrive != null) 
                    {
                        workArrivedRepository.Update(workItem);
                       
                    }
                }
                else
                {
                        workArrivedRepository.Add(workItem); 
                }
                await _unitOfWork.CommitAsync();
                return workItem.Id.ToString();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
           
        }
    }
}
