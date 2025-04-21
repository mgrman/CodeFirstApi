using System.Diagnostics;

namespace CodeFirstApi.Core;

    
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class GenerateHttpClientsAttribute : Attribute
{
    public GenerateHttpClientsAttribute(Type assemblyType)
    {

    }
}

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class GenerateHttpControllersAttribute : Attribute
{
    public GenerateHttpControllersAttribute(Type assemblyType)
    {

    }
}

[AttributeUsage(AttributeTargets.Interface)]
public sealed class GenerateServicesAttribute : Attribute
{
}


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class PersistForPrerendering : Attribute
{
}


/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the specified authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
[DebuggerDisplay("{ToString(),nq}")]
public class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
    /// </summary>
    public AuthorizeAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class with the specified policy.
    /// </summary>
    /// <param name="policy">The name of the policy to require for authorization.</param>
    public AuthorizeAttribute(string policy)
    {
        Policy = policy;
    }

    /// <summary>
    /// Gets or sets the policy name that determines access to the resource.
    /// </summary>
    public string? Policy { get; set; }

    /// <summary>
    /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
    /// </summary>
    public string? Roles { get; set; }

    /// <summary>
    /// Gets or sets a comma delimited list of schemes from which user information is constructed.
    /// </summary>
    public string? AuthenticationSchemes { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Authorize {nameof(Policy)}:{Policy} {nameof(Roles)}:{Roles} {nameof(AuthenticationSchemes)}:{AuthenticationSchemes}";
    }
}