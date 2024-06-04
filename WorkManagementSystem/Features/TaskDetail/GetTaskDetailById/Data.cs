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
        var taskRepo = _unitOfWork.GetRepository<Entities.TaskDetail>().GetAll();
        var departRepo = _unitOfWork.GetRepository<Entities.Department>().GetAll();
        var userRepo = _unitOfWork.GetRepository<Entities.User>().GetAll();
        var workRepo = _unitOfWork.GetRepository<Entities.WorkItem>().GetAll();
        var settingRepo = _unitOfWork.GetRepository<Entities.Setting>().GetAll();

        var work = await (from t in taskRepo.AsNoTracking()
                          join d1 in departRepo.AsNoTracking() on t.DepartmentReceiveId equals d1.Id
                          join w in workRepo.AsNoTracking() on t.WorkItemId equals w.Id
                          join s3 in settingRepo.AsNoTracking() on w.Notation equals s3.Key into sd3
                          from b1 in sd3.DefaultIfEmpty()
                          join u1 in userRepo.AsNoTracking() on t.UserCreateTaskId equals u1.Id
                          join u2 in userRepo.AsNoTracking() on t.LeadershipDirectId equals u2.Id
                          where t.Id == r.TaskId
                          select new TaskDetailResponse
                          {
                              Id = t.Id,
                              Notation = $"{w.ItemId}/{b1.Value}",
                              Content = t.Content,
                              ProcessingStatus = t.ProcessingStatus,
                              Priority = t.Priority,
                              Dealine = t.Dealine.ToFormatString("dd/MM/yyyy"),
                              LeadershipDirectId = t.LeadershipDirectId,
                              DealinePeriodical = t.DealinePeriodical.ToFormatString("dd/MM/yyyy"),
                              DepartmentReceiveId = t.DepartmentReceiveId,
                              // DepartmentSentId = w.DepartmentSentId,
                              IsPeriodical = t.IsPeriodical,
                              Periodical = t.Periodical,
                              KeyWord= t.KeyWord,
                              UserCreateTaskId = t.UserCreateTaskId,
                              UserNameCreateTask = u1.Name,
                              LeadershipDirectName = u2.Name,   
                              DepartmentReceiveName = d1.Name,
                             // DepartmentSentName = d2.Name

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
                                       ActionTime = h.ActionTime.ToFormatString("dd/MM/yyyy HH:mm"),
                                       UserUpdated = u.Name
                                   }).ToListAsync();

            var implemenRepo = _unitOfWork.GetRepository<Implementer>().GetAll();
            var implementions = await (from i in implemenRepo
                                       join u in usersRepo on i.UserReceiveId equals u.Id
                                       where i.IssuesId == r.TaskId
                                       select new Implemention
                                       {
                                           CreatedDate = i.Created.ToFormatString("dd/MM/yyyy HH:mm"),
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
