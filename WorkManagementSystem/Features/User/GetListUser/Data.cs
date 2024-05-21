namespace WorkManagementSystem.Features.User.GetListUser;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<UserModel>> GetUserInfo()
    {
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
        var department = _unitOfWork.GetRepository<Entities.Department>().GetAll();
        var position = _unitOfWork.GetRepository<Entities.Position>().GetAll();
        return await (from u in user.AsNoTracking()
                           join d in department.AsNoTracking() on u.DepartmentId equals d.Id
                           join p in position.AsNoTracking() on u.PositionId equals p.Id
                           where u.Status == StatusEnum.Active
                           orderby u.Name descending
                           select new UserModel
                           {
                               PositionId = p.Id,
                               PositionName = p.Name,
                               DepartmentId = d.Id,
                               DepartmentName = d.Name,
                               Name = u.Name,
                               Id = u.Id,
                               Email = u.Email,
                               Phone = u.Phone,
                               NameAndPosition = $"{u.Name} - {p.Name}"
                           }).ToListAsync();

    }
}
