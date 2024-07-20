using System.Globalization;

namespace WorkManagementSystem.Features.WorkDispatch.PublishWorkDispatch;

public class Mapper : Mapper<Request, Response, Entities.WorkArriveWatting>
{
    public override Entities.WorkArriveWatting ToEntity(Request r) => new()
    {
        KeyWord = r.KeyWord,
        Content = r.Content,
        DepartmentId = r.DepartmentId,
        ItemId = r.ItemId,
        Notation = r.Notation,
        DateIssued = DateTime.ParseExact(r.DateIssued, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        Subjective = r.Subjective,
        LeadershipDirectId = r.LeadershipDirectId,
        Priority = r.Priority,
        DocumentTypeKey = r.DocumentTypeKey,
        IndustryId = r.IndustryId,
        UserSign = r.UserSign,
        Dealine = DateTime.ParseExact(r.Dealine, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        EvictionTime = DateTime.ParseExact(r.EvictionTime, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        SignDay = DateTime.ParseExact(r.SignDay, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        UserCompile = r.UserCompile,
        DepartmentCompile = r.DepartmentCompile,
        TransferType = r.TransferType,
        WorkflowStatus = WorkflowStatusEnum.Waitting
    };
}
