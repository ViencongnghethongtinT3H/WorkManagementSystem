namespace WorkManagementSystem.Features.WorkItem.CreateWorkItem;

public class Mapper : Mapper<Request, Response, Entities.WorkItem>
{
    public override Entities.WorkItem ToEntity(Request r) => new()
    {
        KeyWord = r.KeyWord,
        Content = r.Content,
        DepartmentId = r.DepartmentId,
        ItemId = r.ItemId,
        Notation = r.Notation,
        DateIssued = r.DateIssued,
        Subjective = r.Subjective,
        LeadershipDirectId = r.LeadershipDirectId,
        Priority = r.Priority,
        DocumentTypeKey = r.DocumentTypeKey,        
        ProcessingStatus = ProcessingStatusEnum.None,
        IndustryId = r.IndustryId,
        UserIdCreated = r.UserCreatedId.ToString(),
    };
}
