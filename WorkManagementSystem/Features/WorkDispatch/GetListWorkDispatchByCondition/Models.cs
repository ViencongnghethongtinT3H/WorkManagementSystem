namespace WorkManagementSystem.Features.WorkDispatch.GetListWorkDispatchByCondition;

public class Request
{
    public ContitionWorkflowEnum ContitionWorkflow { get; set; }
    public Guid UserId { get; set; }
    public string? FromDate { get; set; } = string.Empty;
    public string? ToDate { get; set; } = string.Empty;
    public string? Notation {  get; set; } = string.Empty;
    public string? WorkItemNumber { get; set; } = string.Empty;

}

public class Response
{
    public Guid WorkDispatchId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }  // Trích yếu
    public string? WorkflowDispatchNumber { get; set; }   // số văn bản
    public string? Notation { get; set; }  // Số Kí hiệu
    public string Dealine { get; set; }  // hết hạn
    public string LeadershipName { get; set; }   // Tên lãnh đạo chỉ đạo   
   //  public WorkflowStatusEnum WorkflowStatus { get; set; }   // trạng thái của công văn
    public UserWorkflowStatusEnum UserWorkflowStatus { get; set; }   // trạng thái của công văn ứng với người thực hiện (sub menu)
    public UserWorkflowType UserWorkflowType;   // Vai trò thực hiện công văn của user login
    public string NameReceiveCompany { get; set; }
    public DateTime? Created {  get; set; }
}


