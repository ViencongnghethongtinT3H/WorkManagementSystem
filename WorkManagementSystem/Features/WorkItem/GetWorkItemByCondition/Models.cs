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

public class Response : QueryListResponse<WorkItemModel>
{

}
