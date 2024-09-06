using System.Globalization;
using System.Linq;

namespace WorkManagementSystem.Features.WorkArrived.GetListWorkArrivedByConditionHight;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ListResultModel<WorkArriveResponse>> GetListWorkDispatchWattingWork(InputRequest input)
    {
        var userWorkflow = _unitOfWork.GetRepository<UserWorkflow>().GetAll();
        var work = _unitOfWork.GetRepository<Entities.WorkArrived>().GetAll();
        var setting = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
        var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
        var departments = _unitOfWork.GetRepository<Entities.Department>().GetAll();
        var listUserWorkflowType = new List<UserWorkflowType>();

        if (input.MenuStatus.Contains(ContitionWorkflowEnum.Follow))
        {
            listUserWorkflowType.Add(UserWorkflowType.Followers);
        }
        if (input.MenuStatus.Contains(ContitionWorkflowEnum.Proccess))
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
                    join d in departments.AsNoTracking() on w.DepartmentId equals d.Id into dw
                    from d in dw.DefaultIfEmpty()
                    select new
                    {
                        WorkArrived = w,
                        UserWorkflow = uw,
                        Setting = s,
                        User = u,
                        Department = d
                    };

        if (listUserWorkflowType.Any())
        {
            query = query.Where(x => x.UserWorkflow.UserId == input.UserId && listUserWorkflowType.Contains(x.UserWorkflow.UserWorkflowType));
        }
        else
        {
            query = query.Where(x => x.UserWorkflow.UserId == input.UserId);
        }
        if (!string.IsNullOrEmpty(input.FromDate) && !string.IsNullOrEmpty(input.ToDate))
        {
            DateTime fromDate = DateTime.ParseExact(input.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(input.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            query = query.Where(x => (x.WorkArrived.Created >= fromDate && x.WorkArrived.Created <= toDate) || (x.WorkArrived.Dealine >= fromDate && x.WorkArrived.Dealine <= toDate));
        }
        if (!string.IsNullOrEmpty(input.Notation))
        {
            query = query.Where(x => (x.WorkArrived.ItemId + "/" + x.Setting.Value).Contains(input.Notation));
        }
        if (!string.IsNullOrEmpty(input.WorkItemNumber))
        {
            query = query.Where(x => x.WorkArrived.WorkItemNumber.Contains(input.WorkItemNumber));
        }

        if (!string.IsNullOrEmpty(input.DepartmentName))
        {
            query = query.Where(x => x.Department.Id.ToString() == input.DepartmentName);
        }
        if (!string.IsNullOrEmpty(input.LeadName))
        {
            query = query.Where(x => x.User.Id.ToString() == input.LeadName);
        }
        if (!string.IsNullOrEmpty(input.UserCompileName))
        {
            query = query.Where(x => x.User.Id.ToString() == input.UserCompileName);
        }
        if (!string.IsNullOrEmpty(input.SettingName))
        {
            query = query.Where(x => x.Setting.Id.ToString() == input.SettingName);
        }
        var response = query.Select(x => new WorkArriveResponse
        {
            WorkArrivedId = x.WorkArrived.Id,
            Content = x.WorkArrived.Content,
            Notation = $"{x.WorkArrived.ItemId}/{x.Setting.Value}",
            WorkflowArrivedNumber = x.WorkArrived.WorkItemNumber,
            LeadershipName = x.User.Name,
            Dealine = x.WorkArrived.Dealine.ToFormatString("dd/MM/yyyy"),
            UserWorkflowStatus = x.UserWorkflow.UserWorkflowStatus,
            UserWorkflowType = x.UserWorkflow.UserWorkflowType,
            Created = x.UserWorkflow.Created,
            
        }).Skip((input.Page - 1) * input.PageSize).Take(input.PageSize);
        return ListResultModel<WorkArriveResponse>.Create(await response.ToListAsync(), response.Count(), input.Page, input.PageSize);

    }
}
