namespace WorkManagementSystem.Features.WorkDispatch.ApproveWorkDispatch
{
    public class Request
    {
        public Guid UserId { get; set; }
        public Guid WorkFlowId {  get; set; }
        public WorkflowStatusEnum WorkflowStatus { get; set; }
    }
}
