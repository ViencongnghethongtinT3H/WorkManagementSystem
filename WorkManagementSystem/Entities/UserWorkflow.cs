namespace WorkManagementSystem.Entities
{
    // Các user liên quan tới công văn
    public class UserWorkflow : EntityBase
    {
        public Guid WorkflowId { get; set; }
        public Guid UserId { get; set; }
        [StringLength(500)]
        public string? Note { get; set; }
        public UserWorkflowType UserWorkflowType { get; set; }     // vai trò của người xử lý (user login)
        public UserWorkflowStatusEnum UserWorkflowStatus{ get; set; }   // sub menu
        

    }
}
