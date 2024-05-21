namespace WorkManagementSystem.Shared.Domain;

public interface IListQuery<TResponse> where TResponse : notnull
{
   // public List<string> Includes { get; init; }
    public List<FilterModel> Filters { get; init; }
    public List<SortModel> Sorts { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public bool IsPagingEnabled { get; init; }
}
public record FilterModel(string FieldName, string Comparision, string FieldValue);
public record SortModel(string FieldName, SortType SortType);

public enum SortType
{
    Desc = 1,
    Asc = 2,
}
