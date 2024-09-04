using iTextSharp.text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Linq;

namespace WorkManagementSystem.Features.WorkDispatch.GetListWorkDispatchByConditionHight;

public class Data
{
    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ListResultModel<WorkDispatchResponse>> GetListWorkDispatchWattingWork(InputRequest input)
    {
        var userWorkflow = _unitOfWork.GetRepository<UserWorkflow>().GetAll();
        var work = _unitOfWork.GetRepository<Entities.WorkDispatch>().GetAll();
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
                        WorkDispatch = w,
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
            query = query.Where(x => (x.WorkDispatch.Created >= fromDate && x.WorkDispatch.Created <= toDate) || (x.WorkDispatch.Dealine >= fromDate && x.WorkDispatch.Dealine <= toDate));
        }
        if (!string.IsNullOrEmpty(input.Notation))
        {
            query = query.Where(x => (x.WorkDispatch.ItemId + "/" + x.Setting.Value).Contains(input.Notation));
        }
        if (!string.IsNullOrEmpty(input.WorkItemNumber))
        {
            query = query.Where(x => x.WorkDispatch.WorkItemNumber.Contains(input.WorkItemNumber));
        }
        if (!string.IsNullOrEmpty(input.WorkItemNumber))
        {
            query = query.Where(x => x.WorkDispatch.WorkItemNumber.Contains(input.WorkItemNumber));
        }
        if (!string.IsNullOrEmpty(input.DepartmentName))
        {
            query = query.Where(x => x.Department.Name.Contains(input.DepartmentName));
        }
        if (!string.IsNullOrEmpty(input.LeadName))
        {
            query = query.Where(x => x.User.Name.Contains(input.LeadName));
        }
        if (!string.IsNullOrEmpty(input.UserCompileName))
        {
            query = query.Where(x => x.User.Name.Contains(input.UserCompileName));
        }
        if (!string.IsNullOrEmpty(input.SettingName))
        {
            query = query.Where(x => x.Setting.Value.Contains(input.SettingName));
        }
        if (!string.IsNullOrEmpty(input.Subjective))
        {
            query = query.Where(x => x.WorkDispatch.Subjective.Contains(input.Subjective));
        }
        if (!string.IsNullOrEmpty(input.KeyWord))
        {
            query = query.Where(x => x.WorkDispatch.KeyWord.Contains(input.KeyWord));
        }
        var response = query.Select(x => new WorkDispatchResponse
        {
            WorkDispatchId = x.WorkDispatch.Id,
            Content = x.WorkDispatch.Content,
            Notation = $"{x.WorkDispatch.ItemId}/{x.Setting.Value}",
            WorkflowDispatchNumber = x.WorkDispatch.WorkItemNumber,
            LeadershipName = x.User.Name,
            Dealine = x.WorkDispatch.Dealine.ToFormatString("dd/MM/yyyy"),
            UserWorkflowStatus = x.UserWorkflow.UserWorkflowStatus,
            UserWorkflowType = x.UserWorkflow.UserWorkflowType,
            Created = x.UserWorkflow.Created,
        }).Skip((input.Page - 1) * input.PageSize).Take(input.PageSize);
        return ListResultModel<WorkDispatchResponse>.Create(await response.ToListAsync(), response.Count(), input.Page, input.PageSize);

    }
}
