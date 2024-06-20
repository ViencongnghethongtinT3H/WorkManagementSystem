namespace WorkManagementSystem.Features.ReceiveCompany.GetCompany
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Response>> GetList()
        {
            var receiveCompany =  _unitOfWork.GetRepository<Entities.ReceiveCompany>().GetAll().OrderByDescending(p => p.Created);
            var result = await receiveCompany.Select(x => new Response
            {
                Address = x.Address,
                Name = x.Name,
                Created = x.Created,
                Status = x.Status,
                Email = x.Email,
                Fax = x.Fax,
                Id = x.Id,
                Updated = x.Updated,
                UserIdCreated = x.UserIdCreated,
                UserIdUpdated = x.UserIdUpdated,
            }).ToListAsync();
            return result;

        }
    }
}
