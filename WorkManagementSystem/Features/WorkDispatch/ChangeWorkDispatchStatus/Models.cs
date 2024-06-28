namespace WorkManagementSystem.Features.WorkDispatch.ChangeWorkDispatchStatus
{
    public class Request
    {
        public Guid UserId { get; set; }
        public Guid WorkFlowId {  get; set; }
        public WorkflowStatusEnum WorkflowStatus { get; set; }
        public UserWorkflowType UserWorkflowType { get; set; }
        public string? Note { get; set; }
        public RequestImplementer RequestImplementer { get; set; }
    }
}
