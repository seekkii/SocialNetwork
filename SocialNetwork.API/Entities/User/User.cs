using System.Text.Json.Serialization;

namespace SocialNetwork.API.Entities.User;

/// <summary>
/// User class
/// <para>Only handling user's credentials</para>
/// </summary>
public class User : Entity
{
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User's hashed password
    /// </summary>
    [JsonIgnore]
    public string PasswordHash { get; set; }
}
