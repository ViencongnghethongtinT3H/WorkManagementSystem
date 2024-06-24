namespace WorkManagementSystem.Features.WorkArrived.AddWorkArrived
{
    public class Mapper : Mapper<Request, Response, Entities.WorkArrived>
    {
        public override Entities.WorkArrived ToEntity(Request r) => new()
        {
          WorkArrivedStatus = r.WorkArrivedStatus,
          Content = r.Content,
          TransferType = r.TransferType,
          Dealine = r.Dealine,
          DateIssued = r.DateIssued,
          DepartmentId = r.DepartmentId,
          DocumentTypeKey = r.DocumentTypeKey,
          LeadershipDirectId = r.LeadershipDirectId,
          Priority = r.Priority,
          ItemId = r.ItemId,
          WorkItemNumber = r.WorkItemNumber,
          Notation = r.Notation
        };
    }
}
