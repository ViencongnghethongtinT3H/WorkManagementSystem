using WorkManagementSystem.Entities;
using WorkManagementSystem.Infrastructure.UnitOfWork;

namespace WorkManagementSystem.Features.TaskDetail.CreateTaskDetail;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public static Task<string> CreateTaskDetail(Entities.TaskDetail taskDetail)
    {
        return "";

    }
}
