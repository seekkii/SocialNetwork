namespace SocialNetwork.API.Models.User;

/// <summary>
/// Defines what will be send back to Backend after user requests log in
/// </summary>
public class AuthenticateResponse
{
    /// <summary>
    /// User's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's email address
    /// </summary>
    public String Email { get; set; }

    /// <summary>
    /// Jwt token
    /// </summary>
    public String JwtToken { get; set; }
}