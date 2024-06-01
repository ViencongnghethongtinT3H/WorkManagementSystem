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
        var departRepo = _unitOfWork.GetRepository<Entities.Department>().GetAll();
        var userRepo = _unitOfWork.GetRepository<Entities.User>().GetAll();

        var work = await (from w in workRepo.AsNoTracking()
                          join d1 in departRepo.AsNoTracking() on w.DepartmentReceiveId equals d1.Id
                          join d2 in departRepo.AsNoTracking() on w.DepartmentSentId equals d2.Id
                          join u1 in userRepo.AsNoTracking() on w.UserCreateTaskId equals u1.Id
                          join u2 in userRepo.AsNoTracking() on w.LeadershipDirectId equals u2.Id
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
                              KeyWord= w.KeyWord,
                              UserCreateTaskId = w.UserCreateTaskId,
                              UserNameCreateTask = u1.Name,
                              LeadershipDirectName = u2.Name,   
                              DepartmentReceiveName = d1.Name,
                              DepartmentSentName = d2.Name

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
                                       ActionTime = h.ActionTime.ToFormatString("dd/mm/yyyy HH:mm"),
                                       UserUpdated = u.Name
                                   }).ToListAsync();

            var implemenRepo = _unitOfWork.GetRepository<Implementer>().GetAll();
            var implementions = await (from i in implemenRepo
                                       join u in usersRepo on i.UserReceiveId equals u.Id
                                       where i.IssuesId == r.TaskId
                                       select new Implemention
                                       {
                                           CreatedDate = i.Created.ToFormatString("dd/mm/yyyy HH:mm"),
                                           Note = i.Note,
                                           UserReceiveId = i.UserReceiveId,
                                           UserName = u.Name,
                                          ProgressValue = i.ProgressValue,
                                       }).ToListAsync();
            work.Implementions = implementions;
            work.Histories = histories;
        }
        return ResultModel<TaskDetailResponse>.Create(work);
    }
}
