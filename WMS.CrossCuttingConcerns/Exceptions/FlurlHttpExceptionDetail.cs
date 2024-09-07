namespace WMS.CrossCuttingConcerns.Exceptions;

public class FlurlHttpExceptionDetail : Exception
{
    public FlurlHttpExceptionDetail(string url, string message) : base($"Có lỗi xảy ra khi call sang service {url} chi tiết: {message}")
    {
    }
}

public class FlurlHttpTimeOutDetail : Exception
{
    public FlurlHttpTimeOutDetail(string url, string message) : base($"Timeout khi call service: {url} Chitiet: {message}")
    {
    }
}
