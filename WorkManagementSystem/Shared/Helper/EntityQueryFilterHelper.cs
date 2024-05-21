namespace WorkManagementSystem.Shared.Helper;

public static class EntityQueryFilterHelper
{
    public static Func<IQueryable<T>, IQueryable<T>> CreateSort<T>(List<SortModel> sortExprs)
    {
        return source =>
        {
            if (sortExprs == null)
            {
                return source;
            }

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var isFirst = true;
            MethodCallExpression resultExp = null;
            foreach (var sortExpr in sortExprs)
            {
                var property = type.GetProperty(sortExpr.FieldName);
                if (property == null)
                {
                    continue;
                }
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                if (isFirst)
                {
                    resultExp = Expression.Call(typeof(Queryable), sortExpr.SortType == SortType.Desc ? "OrderByDescending" : "OrderBy",
                                                new[] { type, property.PropertyType }, source.Expression,
                                                Expression.Quote(orderByExp));
                    isFirst = false;
                }
                else
                {
                    resultExp = Expression.Call(typeof(Queryable), sortExpr.SortType == SortType.Desc ? "ThenByDescending" : "ThenBy",
                                                new[] { type, property.PropertyType }, resultExp,
                                                Expression.Quote(orderByExp));
                }
            }

            return resultExp == null ? source : source.Provider.CreateQuery<T>(resultExp);
        };
    }

    public static Func<IQueryable<T>, IQueryable<T>> Page<T>(int pageAt = 1, int pageSize = 20)
    {
        if (pageSize != -1)
        {
            var myPage = (pageAt - 1) < 1 ? 0 : pageAt;
            var myPageSize = pageSize <= 0 ? 20 : pageSize;
            return source => source.Skip(myPage * pageSize).Take(myPageSize);
        }

        return source => source;
    }
}
