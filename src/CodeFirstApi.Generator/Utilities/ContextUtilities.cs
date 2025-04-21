using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeFirstApi.Generator;

public static class ContextUtilities
{
    public static void AddSourceFiles(this GeneratorExecutionContext context,
        IEnumerable<(string name, string content)> files)
    {
        foreach (var (name, content) in files)
        {
            context.AddSource(name, content);
        }
    }

    public static IEnumerable<INamedTypeSymbol> GetAllTypeMembers(this IAssemblySymbol assembly)
    {
        return assembly.GlobalNamespace.GetAllTypeMembers();
    }
    public static IEnumerable<INamedTypeSymbol> GetAllTypeMembers(this INamespaceSymbol @namespace)
    {
        foreach (var type in @namespace.GetTypeMembers())
        {
            yield return type;
        }
        foreach (var childNamespace in @namespace.GetNamespaceMembers())
        {
            foreach (var type in childNamespace.GetAllTypeMembers())
            {
                yield return type;
            }
        }
    }
}   