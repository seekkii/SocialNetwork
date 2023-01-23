using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.API.Models.User;

/// <summary>
/// Register model equivalents to register form in Frontend
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    public String Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    [Required]
    public String Password { get; set; }
}
