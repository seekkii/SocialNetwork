namespace SocialNetwork.API.Models.User;

/// <summary>
/// Update credentials model equivalents to update form in Frontend
/// </summary>
public class UpdateCredentialsRequest
{
    /// <summary>
    /// User's email address
    /// </summary>
    public String Email { get; set; }

    /// <summary>
    /// User's new password
    /// </summary>
    public String Password { get; set; }
}