using System.Collections.Generic;
using System.Linq;

namespace CodeFirstApi.Generator;

internal static class StringUtilities
{
    public static string JoinStringWithComma<T>(this IEnumerable<T> items)
    {
        return string.Join(", ", items);
    }

    public static string WrapWithParenthesis(this string text, bool shouldWrap)
    {
        if (shouldWrap)
        {
            return $"({text})";
        }

        return text;
    }

    public static string WrapWithAngleBrackets(this string text)
    {
        return $"<{text}>";
    }
}
