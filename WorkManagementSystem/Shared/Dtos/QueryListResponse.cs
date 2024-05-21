namespace WorkManagementSystem.Shared.Dtos;

public class QueryListResponse<T>
{
    public List<T> Items { get; set; }

    public int Count { get; set; }

}
