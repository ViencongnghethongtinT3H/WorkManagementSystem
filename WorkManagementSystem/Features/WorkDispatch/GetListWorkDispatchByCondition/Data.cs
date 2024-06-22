namespace WorkManagementSystem.Features.WorkDispatch.GetListWorkDispatchByCondition;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ListResultModel<Response>> GetWorkDispatchByCondition(Request input)
    {
        var userWorkflow =  _unitOfWork.GetRepository<UserWorkflow>().GetAll();
        var work = _unitOfWork.GetRepository<Entities.WorkDispatch>().GetAll();
        var setting = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();

        var data = await (from f in userWorkflow.AsNoTracking()
                    join w in work.AsNoTracking() on f.WorkflowId equals w.Id                    
                    join s3 in setting.AsNoTracking() on w.Notation equals s3.Key into sd3
                    from b1 in sd3.DefaultIfEmpty()
                    join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                    from b2 in ud.DefaultIfEmpty()
                    where f.UserId == input.UserId || w.UserCompile == input.UserId
                    select new Response
                    {
                        WorkDispatchId = w.Id,
                        Content = w.Content,
                        Notation = $"{w.ItemId}/{b1.Value}",
                        WorkflowDispatchNumber = w.WorkItemNumber,
                        LeadershipName = b2.Name,
                        Dealine = w.Dealine.ToFormatString("dd/MM/yyyy"),
                        WorkflowStatus = w.WorkflowStatus
                    }).ToListAsync();
        return ListResultModel<Response>.Create(data);
    }
}
