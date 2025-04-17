using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public static class FastFieldCopier
{
    // 型ごとにキャッシュ（初回のみビルド）
    private static readonly Dictionary<Type, Action<object, object>> copierCache = new();

    public static void CopyBaseFields<T>(T source, T destination)
    {
        var type = typeof(T);

        if (!copierCache.TryGetValue(type, out var copier))
        {
            copier = CreateCopier(type);
            copierCache[type] = copier;
        }

        copier(source, destination);
    }

    private static Action<object, object> CreateCopier(Type type)
    {
        var sourceParam = Expression.Parameter(typeof(object), "source");
        var destParam = Expression.Parameter(typeof(object), "destination");

        var sourceCast = Expression.Convert(sourceParam, type);
        var destCast = Expression.Convert(destParam, type);

        var blockExpressions = new List<Expression>();

        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            var sourceField = Expression.Field(sourceCast, field);
            var destField = Expression.Field(destCast, field);

            var assign = Expression.Assign(destField, sourceField);
            blockExpressions.Add(assign);
        }

        var block = Expression.Block(blockExpressions);

        return Expression.Lambda<Action<object, object>>(block, sourceParam, destParam).Compile();
    }
}
