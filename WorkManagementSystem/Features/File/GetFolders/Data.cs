namespace WorkManagementSystem.Features.File.GetFolders;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;

    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Response>> GetList(Request r)
        {
            var fileManagement = await _unitOfWork.GetRepository<Entities.FileManagement>().GetAll().Where(x => x.ParentId == r.ParentId).ToListAsync();
            return fileManagement.Select(fm => new Response
            {
                Id = fm.Id,
                Name = fm.Name,
                ParentId = fm.ParentId,
                FileManagementType = fm.FileManagementType,
            }).ToList();

        }
}
