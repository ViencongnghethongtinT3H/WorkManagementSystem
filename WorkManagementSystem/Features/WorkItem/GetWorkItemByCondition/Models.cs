namespace WorkManagementSystem.Features.WorkItem.GetWorkItemByCondition;
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

public class Response : QueryListResponse<WorkItemResponse>
{
    
}

public class WorkItemResponse 
{
    public Guid WorkItemId { get; set; }
    public Guid? UserId { get; set; }   // người chủ trì công văn
    public Guid? LeadershipId { get; set; }   // người chủ trì công văn
    public string Notation { get; set; }  // số, ký hiệu
    public string? DateIssued { get; set; }  // ngày ban hành
    public string DocumentTypeValue { get; set; }  //  Loại văn bản link tới bảng chung setting
    public string Content { get; set; }   // Trích yếu         
    public string LeadershipDirect { get; set; }   // Tên lãnh đạo chỉ đạo      
    [JsonIgnore]
    public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
    public string PriorityValue { get; set; }  // Độ khẩn cấp
    public string ProcessingStatusValue { get; set; }
    
    [JsonIgnore]
    public ProcessingStatusEnum ProcessingStatus { get; set; }

}