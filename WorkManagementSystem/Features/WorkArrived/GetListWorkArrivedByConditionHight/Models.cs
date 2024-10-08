namespace WorkManagementSystem.Features.WorkArrived.GetListWorkArrivedByConditionHight;

public class InputRequest 
{

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public bool IsPagingEnabled { get; init; } = false;
    public Guid UserId { get; set; }
    public List<ContitionWorkflowEnum>? MenuStatus { get; set; } 
    public string Notation { get; set; } = string.Empty;
    public string UserCompileName { get; set; } = string.Empty;
    public string LeadName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string SettingName { get; set; } = string.Empty;
    public string? FromDate { get; set; } = string.Empty;
    public string? ToDate { get; set; } = string.Empty;
    public string? WorkItemNumber { get; set; } = string.Empty;


}

public class Response : QueryListResponse<WorkArriveResponse>
{

}
public class WorkArriveResponse
{
    public Guid WorkArrivedId { get; set; }
    public string Content { get; set; }  // Trích yếu
    public string? WorkflowArrivedNumber { get; set; }   // số văn bản
    public string? Notation { get; set; }  // Số Kí hiệu
    public string Dealine { get; set; }  // hết hạn
    public string LeadershipName { get; set; }   // Tên lãnh đạo chỉ đạo   
    public WorkArrivedStatus WorkflowStatus { get; set; }   // trạng thái của công văn
    public UserWorkflowStatusEnum UserWorkflowStatus { get; set; }   // trạng thái của công văn ứng với người thực hiện (sub menu)
    public UserWorkflowType UserWorkflowType;   // Vai trò thực hiện công văn của user login
    public DateTime? Created { get; set; }
}
