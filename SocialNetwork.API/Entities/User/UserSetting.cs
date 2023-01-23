namespace SocialNetwork.API.Entities.User;

/// <summary>
/// Universal setting for user
/// <para>User - 1 : 1 - UserSetting</para>
/// <para>Boolean value for all bool attributes:</para>
/// <para>0 - show; 1 - hide</para>
/// </summary>
public class UserSetting : Entity
{
    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User's user interface display language
    /// </summary>
    public String Language { get; set; }

    /// <summary>
    /// Show user's online status
    /// </summary>
    public bool ShowOnlineStatus { get; set; }

    /// <summary>
    /// Show who followed user
    /// </summary>
    public bool ShowFollowers { get; set; }

    /// <summary>
    /// Show who user followed
    /// </summary>
    public bool ShowFollowees { get; set; }

    /// <summary>
    /// Show user's location
    /// </summary>
    public bool ShowLocation { get; set; }

    /// <summary>
    /// Show user's dob
    /// </summary>
    public bool ShowDateOfBirth { get; set; }

    /// <summary>
    /// Show user's gender
    /// </summary>
    public bool ShowGender { get; set; }

    /// <summary>
    /// Future user's posts privacy
    /// </summary>
    public bool PostPrivacy { get; set; }
}
