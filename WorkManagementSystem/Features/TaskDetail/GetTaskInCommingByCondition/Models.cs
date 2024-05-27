namespace WorkManagementSystem.Features.TaskDetail.GetTaskInCommingByCondition;
public class Request
{
    [FromHeader]
    public string Query { get; set; }
}

public class InputRequest : IListQuery<Response>
{
    public List<FilterModel> Filters { get; init; } = new();
    public List<SortModel> Sorts { get; init; } = new();
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public bool IsPagingEnabled { get; init; } = false;
}

public class Response : QueryListResponse<TaskDetailResponse>
{

}

public class TaskDetailResponse
{
    public Guid? Id { get; set; }    // Link tới luồng công văn 
    public Guid? WorkItemId { get; set; }    // Link tới luồng công văn 
    public string Notation { get; set; }    // Link tới luồng công văn 
    public Guid DepartmentSentId { get; set; }    // Đơn vị giao nhiệm vụ
    public Guid DepartmentReceiveId { get; set; }    // Đơn vị nhận nhiệm vụ
    public string Content { get; set; }   // nội dung công việc    
    public string? KeyWord { get; set; }   // từ khoá 
    public string? Dealine { get; set; }  // Thời hạn xử lý       
    public string ProcessingStatusValue { get; set; }  // trạng thái của nhiệm vụ
    public Guid UserReceiveId { get; set; }
    public ProcessingStatusEnum ProcessingStatus { get; set; }  // trạng thái của nhiệm vụ
    public DateTime Created { get; set; }
}
