using System.Collections.Generic;
using System.Linq;

namespace CodeFirstApi.Generator;

internal record OneWayAppInfo(
    TypeInfo app,
    IReadOnlyList<MethodInfo> methods, AuthorizationInfo? authorizationInfo)
{
    public IEnumerable<string> NamespacesToInclude => new string?[] { app.Namespace }
        .Concat(methods.SelectMany(o => o.namespaces))
        .Where(o => !string.IsNullOrEmpty(o))
        .Select(o=>o!)
        .Distinct();
}

internal record TypeInfo(string TypeName, string FullName, string Namespace, bool IsNullable)
{
    public string NullableTypeName => IsNullable ? TypeName + "?" : TypeName;
    public string TypeNameWithoutIPrefix=> TypeName.StartsWith("I")? TypeName.Substring(1) : TypeName;
}

internal record MethodInfo(string name, TypeInfo? returnType, (TypeInfo argType, string argName)[] arguments, AuthorizationInfo? authorizationInfo, bool persistForPrerendering)
{
    public IEnumerable<string> namespaces
    {
        get
        {
            if (returnType != null)
            {
                yield return returnType.Namespace;
            }

            foreach (var arg in arguments)
            {
                yield return arg.argType.Namespace;
            }
        }
    }
}

internal record AuthorizationInfo( string? policy , string? roles,string? authenticationSchemes);