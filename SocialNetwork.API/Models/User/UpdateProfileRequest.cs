namespace SocialNetwork.API.Models.User;

/// <summary>
/// Update user profile, equivalent to user profile form in frontend
/// </summary>
public class UpdateProfileRequest
{
    /// <summary>
    /// User's username
    /// </summary>
    public String Name { get; set; }

    /// <summary>
    /// User's avatar url
    /// </summary>
    public String Avatar { get; set; }

    /// <summary>
    /// User's cover url
    /// </summary>
    public String Cover { get; set; }

    /// <summary>
    /// User's date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// User's address
    /// </summary>
    public String CurrentLocation { get; set; }

    /// <summary>
    /// User's self introduction
    /// </summary>
    public String SelfIntroduction { get; set; }

    /// <summary>
    /// User's gender
    /// <para>0 - Male</para>
    /// <para>1 - Female</para>
    /// <para>2 - Private</para>
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// Check last time profile is updated
    /// </summary>
    public DateTime? Timestamp { get; set; }
}
