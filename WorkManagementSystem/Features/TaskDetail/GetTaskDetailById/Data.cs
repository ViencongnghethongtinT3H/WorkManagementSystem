namespace WorkManagementSystem.Features.TaskDetail.GetTaskDetailById;

public class Data
{

    private readonly IUnitOfWork _unitOfWork;
    public Data(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultModel<TaskDetailResponse>> GetTaskItemById(Request r)
    {
        var workRepo = _unitOfWork.GetRepository<Entities.TaskDetail>().GetAll();

        var work = await (from w in workRepo.AsNoTracking()
                          where w.Id == r.TaskId
                          select new TaskDetailResponse
                          {
                              WorkItemId = w.Id,
                              Content = w.Content,
                              ProcessingStatus = w.ProcessingStatus,
                              Priority = w.Priority,
                              Dealine = w.Dealine.ToFormatString("dd/mm/yyyy"),
                              LeadershipDirectId = w.LeadershipDirectId,
                              DealinePeriodical = w.DealinePeriodical.ToFormatString("dd/mm/yyyy"),
                              DepartmentReceiveId = w.DepartmentReceiveId,
                              DepartmentSentId = w.DepartmentSentId,
                              IsPeriodical = w.IsPeriodical,
                              Periodical = w.Periodical,
                              KeyWord= w.KeyWord

                          }).FirstOrDefaultAsync();

        if (work is not null)
        {
            var historyRepo = _unitOfWork.GetRepository<Entities.History>().GetAll();
            var usersRepo = _unitOfWork.GetRepository<Entities.User>().GetAll();
            var histories = await (from h in historyRepo
                                   join u in usersRepo on h.UserId equals u.Id
                                   where h.IssueId == r.TaskId
                                   orderby h.Created descending
                                   select new HistoryListModel
                                   {
                                       ActionContent = h.actionContent,
                                       ActionTime = h.ActionTime.ToFormatString("dd/mm/yyyy"),
                                       UserUpdated = u.Name
                                   }).ToListAsync();

            var implemenRepo = _unitOfWork.GetRepository<Implementer>().GetAll();
            var implementions = await (from i in implemenRepo
                                       join u in usersRepo on i.UserReceiveId equals u.Id
                                       where i.IssuesId == r.TaskId
                                       select new Implemention
                                       {
                                           CreatedDate = i.Created.ToFormatString("dd/mm/yyyy"),
                                           Note = i.Note,
                                           UserReceiveId = i.UserReceiveId,
                                           UserName = u.Name,
                                       }).ToListAsync();
            work.Implementions = implementions;
            work.Histories = histories;
        }
        return ResultModel<TaskDetailResponse>.Create(work);
    }
}
