namespace WorkManagementSystem.Shared.Dtos;
public record ResultModel<T>(T Data, bool IsError = false, string ErrorMessage = default!, int Status = StatusCodes.Status200OK) where T : notnull
{
    public static ResultModel<T> Create(T? data, bool isError = false, string errorMessage = default!, int status = StatusCodes.Status200OK)
    {
        return new(data, isError, errorMessage, status);
    }
}

public record ListResultModel<T>(List<T> Items, long TotalItems, int Page, int PageSize) where T : notnull
{
    public static ListResultModel<T> Create(List<T> items, long totalItems = 0, int page = 1, int pageSize = 20)
    {
        return new(items, totalItems, page, pageSize);
    }
}

public record ListResultSelectModel<T>(List<T> Items) where T : notnull
{
    public static ListResultSelectModel<T> Create(List<T> items)
    {
        return new(items);
    }
}
