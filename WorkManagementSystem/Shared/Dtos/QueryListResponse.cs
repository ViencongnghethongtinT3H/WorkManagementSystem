namespace WorkManagementSystem.Shared.Dtos;

public class QueryListResponse<T>
{
    public IEnumerable<T> Items { get; set; }

    public int Count { get; set; }

}
