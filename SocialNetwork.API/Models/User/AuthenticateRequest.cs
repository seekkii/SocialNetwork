using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.API.Models.User;

/// <summary>
/// Auth model equivalents to Log in form in Frontend
/// </summary>
public class AuthenticateRequest
{
    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    [Required]
    public string Password { get; set; }
}
