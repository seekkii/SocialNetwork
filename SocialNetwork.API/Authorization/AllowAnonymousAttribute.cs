namespace SocialNetwork.API.Authorization;

/// <summary>
/// Attribute to use in controllers which do not require jwt token authentication
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }
