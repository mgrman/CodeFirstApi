using System;
using System.Collections.Generic;

namespace CodeFirstApi.Generator;

public static class LinqUtilities
{
    public static bool TryGet<T>(this IEnumerable<T> source,Func<T,bool> predicate,  out T result)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                result = item;
                return true;
            }
        }
        result = default;
        return false;
    }

    public static IEnumerable<(T, int)> EnumerateWithIndex<T>(this IEnumerable<T> source)
    {
        var index = 0;
        foreach (var item in source)
        {
            yield return (item, index);
            index++;
        }
    }
}
