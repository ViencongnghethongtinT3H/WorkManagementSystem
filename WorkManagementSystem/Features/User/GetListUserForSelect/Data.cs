using WorkManagementSystem.Shared.Dtos;

namespace WorkManagementSystem.Features.User.GetListUserForSelect;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel<List<UserModel>>> GetUserInfo(Request r)
    {
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
        var position = _unitOfWork.GetRepository<Position>().GetAll();
        var department = _unitOfWork.GetRepository<Entities.Department>().GetAll();

        var data = await (from u in user.AsNoTracking()
                      join p in position.AsNoTracking() on u.PositionId equals p.Id
                      join d in department.AsNoTracking() on u.DepartmentId equals d.Id
                          where u.Status == StatusEnum.Active && (r.DepartmentId == null || u.DepartmentId == r.DepartmentId.Value)
                      orderby u.Name descending
                      select new UserModel
                      {
                          PositionId = p.Id,
                          PositionName = p.Name,
                          Name = u.Name,
                          Id = u.Id,
                          NameAndPosition = $"{u.Name} - {p.Name}",
                          DeparmentName = d.Name,
                         
                      }).ToListAsync();
        return ResultModel<List<UserModel>>.Create(data);
    }
}
