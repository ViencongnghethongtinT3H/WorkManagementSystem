using FastEndpoints;
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

    public async Task<string> CreateTaskDetail(Entities.TaskDetail taskDetail)
    {
        var movieRepository = _unitOfWork.GetRepository<Entities.TaskDetail>();
        movieRepository.Add(taskDetail);
        await _unitOfWork.CommitAsync();
        return taskDetail.Id.ToString();

    }
}
