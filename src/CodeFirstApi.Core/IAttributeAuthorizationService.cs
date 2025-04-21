namespace CodeFirstApi.Core;

public interface IAttributeAuthorizationService
{
    ValueTask ValidateAsync(string policy, string roles, string authenticationSchemes);
}
