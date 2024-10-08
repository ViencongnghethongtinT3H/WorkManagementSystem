using Flurl;
using Flurl.Http;
namespace WMS.CrossCuttingConcerns.FluentHTTP;

public static class FluentHTTPHelper<T>
{
    public static async Task<T> PostAsync(string url, object headers, object content, string segments = "")
    {
        if (!string.IsNullOrEmpty(segments))
        {
            return await url.AppendPathSegment(segments)
            .WithHeaders(headers)
            .PostJsonAsync(content)
            .ReceiveJson<T>();
        }
        else
        {
            return await url.WithHeaders(headers)
            //   .WithHeaders(headers)
            .PostJsonAsync(content)
            .ReceiveJson<T>();
        }

    }

    public static async Task<T> PostAsyncWithBasicAuthen(string url, object headers, object content,
        string userName, string password)
    {
        return await url.WithHeaders(headers)
            .WithBasicAuth(userName, password)
            .PostJsonAsync(content)
            .ReceiveJson<T>();
    }

    public static async Task<T> PostAsyncWithBearerToken(string url, string token, object headers, object content)
    {
        return await url.WithHeaders(headers)
            .WithOAuthBearerToken(token)
            .PostJsonAsync(content)
            .ReceiveJson<T>();
    }

    public static async Task<T> GetAsync(string url, string token, object content = null, string segments = "")
    {
        T result;
        if (!string.IsNullOrEmpty(segments))
        {
            result = await url.WithOAuthBearerToken(token).AppendPathSegment(segments).GetJsonAsync<T>();
        }
        else
        {
            if (content != null)
            {
                result = await url.WithOAuthBearerToken(token).SetQueryParams(new { Ids = content }).GetJsonAsync<T>();
            }
            else
            {
                result = await url.WithOAuthBearerToken(token).GetJsonAsync<T>();
            }
        }
        return result;
    }


    public static async Task<T> GetAsync(string url, string segments = "")
    {
        T result;

        if (!string.IsNullOrEmpty(segments))
        {
            result = await url.AppendPathSegment(segments).GetJsonAsync<T>();
        }
        else
        {
            result = await url.GetJsonAsync<T>();
        }

        return result;
    }


    public static async Task GetStringAsync(string url, string serviceName)
    {
        try
        {
            var result = await url.GetStringAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
}
