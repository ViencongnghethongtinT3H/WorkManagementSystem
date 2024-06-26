namespace WorkManagementSystem.Features.WorkArrived.ChangeWorkArrivedStatus
{
    public class Request
    {
        public Guid UserId { get; set; }
        public Guid WorkArriveId { get; set; }
        public WorkArrivedStatus WorkArrivedStatus { get; set; }
    }
}
