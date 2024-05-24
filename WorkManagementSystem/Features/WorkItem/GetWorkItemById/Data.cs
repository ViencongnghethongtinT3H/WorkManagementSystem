
namespace WorkManagementSystem.Features.WorkItem.GetWorkItemById;

public class Data
{

    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultModel<WorkItemDetailResponse>> GetWorkItemById(Request r)
    {
        var workRepo = _unitOfWork.GetRepository<Entities.WorkItem>().GetAll();
        var settingRepo = _unitOfWork.GetRepository<Entities.Setting>().GetAll();

        var work = await (from w in workRepo.AsNoTracking()
                          join s3 in settingRepo.AsNoTracking() on w.Notation equals s3.Key into sd3
                          from b1 in sd3.DefaultIfEmpty()
                          where w.Id == r.WorkId
                          select new WorkItemDetailResponse
                          {
                              WorkItemId = w.Id,
                              WorkItemNumber = w.WorkItemNumber,
                              Content = w.Content,
                              Notation = $"{w.ItemId}/{b1.Value}",
                              ProcessingStatus = w.ProcessingStatus,
                              DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy"),
                              Priority = w.Priority,
                              UserId = w.UserId,
                              Subjective = w.Subjective,
                              DepartmentId = w.DepartmentId,
                              Dealine = w.Dealine,
                              EvictionTime = w.EvictionTime,
                              IndustryId = w.IndustryId,
                              LeadershipDirectId = w.LeadershipDirectId
                          }).FirstOrDefaultAsync();

        if (work is not null)
        {
            var historyRepo = _unitOfWork.GetRepository<Entities.History>().GetAll();
            var usersRepo = _unitOfWork.GetRepository<Entities.User>().GetAll();
            var histories = await (from h in historyRepo
                                   join u in usersRepo on h.UserId equals u.Id
                                   where h.IssueId == r.WorkId
                                   orderby h.Created descending
                                   select new HistoryListModel
                                   {
                                       ActionContent = h.actionContent,
                                       ActionTime = h.ActionTime,
                                       UserUpdated = u.Name
                                   }).ToListAsync();

            var implemenRepo = _unitOfWork.GetRepository<Implementer>().GetAll();
            var departmentRepo = _unitOfWork.GetRepository<Entities.Department>().GetAll();
            var implementions = await (from i in implemenRepo
                                       join h in departmentRepo on i.DepartmentReceiveId equals h.Id
                                       join u in usersRepo on i.UserReceiveId equals u.Id
                                       where i.IssuesId == r.WorkId
                                       select new Implemention
                                       {
                                           CreatedDate = i.Created,
                                           DepartmentReceiveId = i.DepartmentReceiveId,
                                           DepartmentReceiveName = h.Name,
                                           Note = i.Note,
                                           UserReceiveId = i.UserReceiveId,
                                           UserName = u.Name,
                                       }).ToListAsync();
            work.Implementions = implementions;
            work.Histories = histories;
        }
        return ResultModel<WorkItemDetailResponse>.Create(work);
    }
}
