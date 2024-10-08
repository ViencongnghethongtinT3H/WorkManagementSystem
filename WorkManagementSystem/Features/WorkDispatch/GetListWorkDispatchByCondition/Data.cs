using System.Globalization;

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
        else if (input.ContitionWorkflow == ContitionWorkflowEnum.Proccess)
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
                        WorkDispatch = w,
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
            query = query.Where(x => (x.WorkDispatch.ItemId + "/" + x.Setting.Value).Contains(input.Notation));
        }
        if (!string.IsNullOrEmpty(input.WorkItemNumber))
        {
            query = query.Where(x => x.WorkDispatch.WorkItemNumber.Contains(input.WorkItemNumber));
        }
        if (!string.IsNullOrEmpty(input.FromDate) && !string.IsNullOrEmpty(input.ToDate))
        {
            DateTime fromDate = DateTime.ParseExact(input.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(input.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            query = query.Where(x => x.WorkDispatch.Created >= fromDate && x.WorkDispatch.Created <= toDate);
        }

        var data = await query.OrderByDescending(x => x.WorkDispatch.Created).ToListAsync();

        var response = data.Select(x => new Response
        {
            WorkDispatchId = x.WorkDispatch.Id,
            UserId = x.UserWorkflow.UserId,
            Content = x.WorkDispatch.Content,
            Notation = $"{x.WorkDispatch.ItemId}/{x.Setting.Value}",
            WorkflowDispatchNumber = x.WorkDispatch.WorkItemNumber,
            LeadershipName = x.User.Name,
            Dealine = x.WorkDispatch.Dealine.ToFormatString("dd/MM/yyyy"),
            UserWorkflowStatus = x.UserWorkflow.UserWorkflowStatus,
            UserWorkflowType = x.UserWorkflow.UserWorkflowType,
            Created = x.UserWorkflow.Created
        }).ToList();

        return ListResultModel<Response>.Create(response);
    }

}