using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public static class FastInstanceAdder
{
    private static readonly Dictionary<Type, List<IFieldAccessor>> accessorCache = new();

    public static T AddInstances<T>(T[] instances) where T : new()
    {
        Type type = typeof(T);
        if (!accessorCache.TryGetValue(type, out var accessors))
        {
            accessors = BuildAccessors<T>();
            accessorCache[type] = accessors;
        }

        T newInstance = new();

        foreach (var accessor in accessors)
        {
            accessor.Aggregate(instances, newInstance);
        }

        return newInstance;
    }

    private static List<IFieldAccessor> BuildAccessors<T>()
    {
        var accessors = new List<IFieldAccessor>();
        var type = typeof(T);
        var param = Expression.Parameter(typeof(T), "x");

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (field.FieldType == typeof(int))
                accessors.Add(new FieldAccessor<int>(field));
            else if (field.FieldType == typeof(float))
                accessors.Add(new FieldAccessor<float>(field));
            else if (field.FieldType == typeof(bool))
                accessors.Add(new FieldAccessor<bool>(field));
            else if (field.FieldType == typeof(string))
                accessors.Add(new FieldAccessor<string>(field));
        }

        return accessors;
    }

    // インターフェースで共通化
    private interface IFieldAccessor
    {
        void Aggregate<T>(T[] instances, T target);
    }

    // 各型ごとのフィールドアクセサ
    private class FieldAccessor<TField> : IFieldAccessor
    {
        private readonly Func<object, TField> getter;
        private readonly Action<object, TField> setter;
        private readonly Func<TField, TField, TField> aggregator;

        public FieldAccessor(FieldInfo field)
        {
            getter = BuildGetter(field);
            setter = BuildSetter(field);
            aggregator = BuildAggregator(field.FieldType);
        }

        public void Aggregate<T>(T[] instances, T target)
        {
            TField result = default;
            bool initialized = false;

            foreach (var inst in instances)
            {
                var value = getter(inst);
                if (!initialized)
                {
                    result = value;
                    initialized = true;
                }
                else
                {
                    result = aggregator(result, value);
                }

                // bool型なら true で即抜け（最速化）
                if (typeof(TField) == typeof(bool) && (bool)(object)result)
                    break;
            }

            setter(target, result);
        }

        private static Func<object, TField> BuildGetter(FieldInfo field)
        {
            var param = Expression.Parameter(typeof(object), "instance");
            var casted = Expression.Convert(param, field.DeclaringType);
            var fieldAccess = Expression.Field(casted, field);
            var convertResult = Expression.Convert(fieldAccess, typeof(TField));
            return Expression.Lambda<Func<object, TField>>(convertResult, param).Compile();
        }

        private static Action<object, TField> BuildSetter(FieldInfo field)
        {
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var valueParam = Expression.Parameter(typeof(TField), "value");

            var castedInstance = Expression.Convert(instanceParam, field.DeclaringType);
            var castedValue = Expression.Convert(valueParam, field.FieldType);

            var assign = Expression.Assign(Expression.Field(castedInstance, field), castedValue);
            return Expression.Lambda<Action<object, TField>>(assign, instanceParam, valueParam).Compile();
        }

        private static Func<TField, TField, TField> BuildAggregator(Type fieldType)
        {
            if (fieldType == typeof(int))
                return (a, b) => (TField)(object)((int)(object)a + (int)(object)b);
            if (fieldType == typeof(float))
                return (a, b) => (TField)(object)((float)(object)a + (float)(object)b);
            if (fieldType == typeof(bool))
                return (a, b) => (TField)(object)((bool)(object)a || (bool)(object)b);
            if (fieldType == typeof(string))
                return (a, b) => (TField)(object)(((string)(object)a) + "+" + ((string)(object)b));
            throw new NotSupportedException("Unsupported type: " + fieldType);
        }
    }
}
