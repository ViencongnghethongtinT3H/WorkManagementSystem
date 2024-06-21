namespace WorkManagementSystem.Features.WorkDispatch.GetListWorkDispatchByCondition;

public class Request
{
    public ContitionWorkflowEnum ContitionWorkflow { get; set; }
    public Guid UserId { get; set; }
}

public class Response
{
    public string Content { get; set; }  // Trích yếu
    public string? WorkflowDispatchNumber { get; set; }   // số văn bản
    public string? Notation { get; set; }  // Số Kí hiệu
    public string Dealine { get; set; }  // Số Kí hiệu
    public string LeadershipName { get; set; }   // Tên lãnh đạo chỉ đạo   
}


