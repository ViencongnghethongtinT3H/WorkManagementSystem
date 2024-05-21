namespace WorkManagementSystem.Shared.Extensions;

public static class Extensions
{
    public static TResult SafeGetListQuery<TResult, TResponse>(this HttpContext httpContext, string query)
           where TResult : IListQuery<TResponse>, new()
    {
        var queryModel = new TResult();
        if (!(string.IsNullOrEmpty(query) || query == "{}"))
        {
            queryModel = JsonConvert.DeserializeObject<TResult>(query);
        }

        httpContext?.Response.Headers.Add("x-query",
            JsonConvert.SerializeObject(queryModel,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        return queryModel;
    }    
}
