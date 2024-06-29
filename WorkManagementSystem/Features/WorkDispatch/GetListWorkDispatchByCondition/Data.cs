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
        var userWorkflow = _unitOfWork.GetRepository<UserWorkflow>().GetAll();
        var work = _unitOfWork.GetRepository<Entities.WorkDispatch>().GetAll();
        var setting = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
        var listUserWorkflowType = new List<UserWorkflowType>();
        if (input.ContitionWorkflow == ContitionWorkflowEnum.Follow)
        {
            listUserWorkflowType.Add(UserWorkflowType.Followers);
        }
        else if (input.ContitionWorkflow== ContitionWorkflowEnum.Proccess)
        {
            listUserWorkflowType.Add(UserWorkflowType.Signarture);
            listUserWorkflowType.Add(UserWorkflowType.Submit);
            listUserWorkflowType.Add(UserWorkflowType.Implementer);
        }

        var data = await (from w in work.AsNoTracking()
                           join f in userWorkflow.AsNoTracking() on w.Id equals f.WorkflowId into userW
                           from uw in userW.DefaultIfEmpty()
                           join s3 in setting.AsNoTracking() on w.Notation equals s3.Key into sd3
                           from b1 in sd3.DefaultIfEmpty()
                           join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                           from b2 in ud.DefaultIfEmpty()
                           where uw.UserId == input.UserId && listUserWorkflowType.Contains(uw.UserWorkflowType)
                          select new Response
                           {
                               WorkDispatchId = w.Id,
                               Content = w.Content,
                               Notation = $"{w.ItemId}/{b1.Value}",
                               WorkflowDispatchNumber = w.WorkItemNumber,
                               LeadershipName = b2.Name,
                               Dealine = w.Dealine.ToFormatString("dd/MM/yyyy"),
                               WorkflowStatus = w.WorkflowStatus,
                               UserWorkflowStatus = uw.UserWorkflowStatus
                           }).ToListAsync();
        return ListResultModel<Response>.Create(data);
    }
}
