using System.Globalization;

namespace WorkManagementSystem.Features.WorkArrived.GetListWorkArrivedByCondition;

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
        var work = _unitOfWork.GetRepository<Entities.WorkArrived>().GetAll();
        var setting = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
        var listUserWorkflowType = new List<UserWorkflowType>();

        if (input.MenuStatus == ContitionWorkflowEnum.Follow)
        {
            listUserWorkflowType.Add(UserWorkflowType.Followers);
        }
        else if (input.MenuStatus == ContitionWorkflowEnum.Proccess)
        {
            listUserWorkflowType.Add(UserWorkflowType.Signarture);
            listUserWorkflowType.Add(UserWorkflowType.Submit);
            listUserWorkflowType.Add(UserWorkflowType.Implementer);
        }

        var query = from w in work.AsNoTracking()
                    join uw in userWorkflow.AsNoTracking() on w.Id equals uw.WorkflowId into userW
                    from uw in userW.DefaultIfEmpty()
                    join s in setting.AsNoTracking() on w.Notation equals s.Key into sd
                    from s in sd.DefaultIfEmpty()
                    join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                    from u in ud.DefaultIfEmpty()
                    select new
                    {
                        WorkArrived = w,
                        UserWorkflow = uw,
                        Setting = s,
                        User = u,
                    };

        if (listUserWorkflowType.Any())
        {
            query = query.Where(x => x.UserWorkflow.UserId == input.UserId && listUserWorkflowType.Contains(x.UserWorkflow.UserWorkflowType));
        }
        else
        {
            query = query.Where(x => x.UserWorkflow.UserId == input.UserId);
        }
        if (!string.IsNullOrEmpty(input.Notation))
        {
            query = query.Where(x => (x.WorkArrived.ItemId + "/" + x.Setting.Value).Contains(input.Notation));
        }
        if (!string.IsNullOrEmpty(input.WorkItemNumber))
        {
            query = query.Where(x => (x.WorkArrived.WorkItemNumber).Contains(input.WorkItemNumber));
        }
        if (!string.IsNullOrEmpty(input.FromDate) && !string.IsNullOrEmpty(input.ToDate))
        {
            DateTime fromDate = DateTime.ParseExact(input.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(input.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            query = query.Where(x => (x.WorkArrived.Created >= fromDate && x.WorkArrived.Created <= toDate) || (x.WorkArrived.Dealine >= fromDate && x.WorkArrived.Dealine <= toDate));
        }
        var data = await query.OrderByDescending(x => x.WorkArrived.Created).ToListAsync();

        var response = data.Select(x => new Response
        {
            WorkArrivedId = x.WorkArrived.Id,
            UserId = x.UserWorkflow.UserId,
            Content = x.WorkArrived.Content,
            Notation = $"{x.WorkArrived.ItemId}/{x.Setting?.Value}",  // Handle potential null value
            WorkflowArrivedNumber = x.WorkArrived.WorkItemNumber,
            LeadershipName = x.User?.Name,  // Handle potential null value
            Dealine = x.WorkArrived.Dealine.ToFormatString("dd/MM/yyyy"),
            WorkflowStatus = x.WorkArrived.WorkArrivedStatus,
            UserWorkflowStatus = x.UserWorkflow.UserWorkflowStatus,
            UserWorkflowType = x.UserWorkflow.UserWorkflowType,
            Created = x.WorkArrived.Created
        }).ToList();

        return ListResultModel<Response>.Create(response);
    }
}
