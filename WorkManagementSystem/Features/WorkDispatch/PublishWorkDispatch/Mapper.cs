namespace WorkManagementSystem.Features.WorkDispatch.PublishWorkDispatch;

public class Mapper : Mapper<Request, Response, Entities.WorkArriveWatting>
{
    public override WorkArriveWatting ToEntity(Request r) => new()
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
        IndustryId = r.IndustryId,
        UserSign = r.UserSign,
        Dealine = r.Dealine,
        EvictionTime = r.EvictionTime,
        SignDay = r.SignDay,
        UserCompile = r.UserCompile,
        DepartmentCompile = r.DepartmentCompile,
        TransferType = r.TransferType,
        WorkflowStatus = WorkflowStatusEnum.Waitting
    };
}
