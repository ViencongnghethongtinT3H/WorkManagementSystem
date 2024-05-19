namespace WorkManagementSystem.Features.TaskDetail.CreateTaskDetail;

public class Mapper : Mapper<Request, Response, Entities.TaskDetail>
{
    public override async Task<Entities.TaskDetail> ToEntityAsync(Request r, CancellationToken ct = default) => new()
    {
       KeyWord = r.KeyWord,
       DepartmentReceiveId = r.DepartmentReceiveId,
       Content = r.Content,
       Priority = r.Priority,
       Periodical = r.Periodical,
       WorkItemId = r.WorkItemId,
       DealinePeriodical = r.DealinePeriodical,
       IsPeriodical = r.IsPeriodical,
       DepartmentSentId = r.DepartmentSentId,
       LeadershipDirectId = r.LeadershipDirectId,
       UserCreateTaskId = r.UserCreateTaskId,
       ProcessingStatus = Entities.Enums.ProcessingStatusEnum.None,
       Dealine = r.Dealine,
       Status = Entities.Enums.StatusEnum.Active
    };
}
