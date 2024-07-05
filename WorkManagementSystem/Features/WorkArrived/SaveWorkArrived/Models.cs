namespace WorkManagementSystem.Features.WorkArrived.SaveWorkArrived
{
    public class Request
    {
        public Guid UserId { get; set; }
        public Guid WorkArriveId { get; set; }
        public Guid LeadershipDirectId { get; set; }
    }
}
