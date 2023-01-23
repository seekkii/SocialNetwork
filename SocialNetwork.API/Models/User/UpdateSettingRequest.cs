namespace SocialNetwork.API.Models.User;

/// <summary>
/// Update user universal setting in application, equivalent to user setting form in frontend
/// </summary>
public class UpdateSettingRequest
{
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
