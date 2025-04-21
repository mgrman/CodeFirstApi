using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeFirstApi.Generator;

[Generator]
public class CodeFirstApiGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a syntax receiver that will be created for each generation pass
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context) // Implement Execute method
    {
        // retrieve the populated receiver 
        if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
        {
            return;
        }

        if (!receiver.ShouldGenerate)
        {
            return;
        }

        List<MethodInfo> GetMethodInfos(INamedTypeSymbol? o)
        {
            var methods = o.GetMembers()
                .OfType<IMethodSymbol>()
                .Select(m => GetMethodInfo(m, context.Compilation))
                .Where(m => m != null)
                .ToList();
            return methods;
        }
        
        IEnumerable<OneWayAppInfo> ConcreteTypeToOneWayAppInfoWithOwner(TypeSyntax o)
        {
            
            var assemblyType= context.Compilation.GetSemanticModel(o.SyntaxTree).GetTypeInfo(o).Type as INamedTypeSymbol;


            var typeMembers = assemblyType.ContainingAssembly.GetAllTypeMembers();

            foreach (var typeMember in typeMembers)
            {
                if (!typeMember.GetAttributes()
                    .Any(a=>
                    a.AttributeClass.Name== "GenerateServicesAttribute"))
                {
                    continue;
                }
                
                var authInfo=GetAuthorizationInfo(typeMember);

                var methods = GetMethodInfos(typeMember);

                yield return new OneWayAppInfo(GetTypeInfo(typeMember, context.Compilation), methods, authInfo);
            }
        }

        
        foreach (var appInfo in receiver.HttpClientTypes.SelectMany(ConcreteTypeToOneWayAppInfoWithOwner))
        {
            var files = CodeFirstApiGenerator_HttpClientApi.Generate(appInfo);
            context.AddSourceFiles(files);
        }

        if (receiver.HttpClientTypes.Any())
        {
            var extensionFiles =
                CodeFirstApiGenerator_HttpClientApi.GenerateServicesMethod(
                    receiver.HttpClientTypes.SelectMany(ConcreteTypeToOneWayAppInfoWithOwner));
            context.AddSourceFiles(extensionFiles);
        }

        foreach (var appInfo in receiver.HttpControllerTypes.SelectMany(ConcreteTypeToOneWayAppInfoWithOwner))
        {
            var files = CodeFirstApiGenerator_HttpControllerApi.Generate(appInfo);

            context.AddSourceFiles(files);
        }

        if (receiver.HttpControllerTypes.Any())
        {
            var extension2Files =
                CodeFirstApiGenerator_HttpControllerApi.GenerateServicesMethod(
                    receiver.HttpControllerTypes.SelectMany(ConcreteTypeToOneWayAppInfoWithOwner));
            context.AddSourceFiles(extension2Files);
        }

    }

    private static TypeInfo GetTypeInfo(INamedTypeSymbol typeSymbol, Compilation compilation)
    {
        var fullName= typeSymbol.ToDisplayString();
        var typeName = typeSymbol.Name;
        bool isNullable = typeSymbol.NullableAnnotation == NullableAnnotation.Annotated;
        if (typeName == nameof(Nullable))
        {
            isNullable = true;
            typeName = typeSymbol.TypeArguments[0].Name;
        }


        return new TypeInfo(typeName, fullName, typeSymbol.ContainingNamespace.ToDisplayString(), isNullable);
    }

    private static MethodInfo GetMethodInfo(IMethodSymbol method, Compilation compilation)
    {
        var name = method.Name;

        var authorizationInfo = GetAuthorizationInfo(method);

        var persistForPrerendering = method.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == "PersistForPrerendering") != null;

        INamedTypeSymbol? returnType;
        if (method.ReturnType is INamedTypeSymbol identifierNameSyntax)
        {
            if (identifierNameSyntax.Name != "ValueTask")
            {
                throw new InvalidOperationException();
            }
            if (identifierNameSyntax.IsGenericType  && identifierNameSyntax.TypeArguments.Length==1)
            {
                returnType = identifierNameSyntax.TypeArguments[0] as INamedTypeSymbol;
            }
            else
            {
                returnType = null;
            }
        }
        else
        {
            throw new InvalidOperationException();
        }

        var arguments = new List<(TypeInfo argType, string argName)>();
        foreach (var arg in method.Parameters)
        {
            var argName = arg.Name;
            var argType = GetTypeInfo(arg.Type as INamedTypeSymbol, compilation);
            arguments.Add((argType, argName)); 
        }

        return new MethodInfo(name, returnType==null?null:GetTypeInfo(returnType , compilation), arguments.ToArray(),authorizationInfo, persistForPrerendering);
    }

    private static AuthorizationInfo? GetAuthorizationInfo(ISymbol symbol)
    {
        var attrs=symbol.GetAttributes();
        var authAttr=attrs.FirstOrDefault(a=>a.AttributeClass.Name=="AuthorizeAttribute");
        AuthorizationInfo authorizationInfo=null;
        if (authAttr != null)
        {
            string? policy=authAttr.ConstructorArguments.Length==1?authAttr.ConstructorArguments.First().ToCSharpString() : authAttr.NamedArguments.TryGet(o => o.Key == "Policy", out var policyPair)?policyPair.Value.ToCSharpString():null;
            string? roles = authAttr.NamedArguments.TryGet(o => o.Key == "Roles", out var rolesPair)?rolesPair.Value.ToCSharpString():null;
            string? authenticationSchemes = authAttr.NamedArguments.TryGet(o => o.Key == "AuthenticationSchemes", out var authenticationSchemesPair)?authenticationSchemesPair.Value.ToCSharpString():null;        
         
            authorizationInfo=new AuthorizationInfo(policy,roles,authenticationSchemes);
        }

        return authorizationInfo;
    }


    /// <summary>
    ///     Created on demand before each generation pass
    /// </summary>
    private class SyntaxReceiver : ISyntaxContextReceiver
    {
        public bool ShouldGenerate => HttpClientTypes.Count > 0 || HttpControllerTypes.Count > 0;

        public List<TypeSyntax> HttpClientTypes { get; } = new();
        public List<TypeSyntax > HttpControllerTypes { get; } = new();
        
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            // TODO could be usefull to even do this automatically for all referenced projects, and then pick Controller or Client based on SDK type (wasm blazor vs web)
            
            
            if (context.Node is AttributeSyntax attribute)
            {
                if (attribute.Name.ToFullString() == "GenerateHttpControllers")
                {
                    var assemblyType = (attribute.ArgumentList.Arguments[0].Expression as TypeOfExpressionSyntax).Type;
                    HttpControllerTypes.Add(assemblyType);
                }
                if (attribute.Name.ToFullString() == "GenerateHttpClients")
                {
                    var assemblyType = (attribute.ArgumentList.Arguments[0].Expression as TypeOfExpressionSyntax).Type;
                    HttpClientTypes.Add(assemblyType);
                }
            }
        }
    }
}
