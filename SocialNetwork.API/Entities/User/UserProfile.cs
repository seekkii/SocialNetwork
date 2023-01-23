namespace SocialNetwork.API.Entities.User;

/// <summary>
/// User's profile, not credentials
/// <para>User - 1 : 1 - UserProfile</para>
/// </summary>
public class UserProfile : Entity
{
    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }

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
    /// User's current location
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
}
