namespace SocialNetwork.API.Entities.Chat;

/// <summary>
/// Membership of a group chat
/// </summary>
public class ChatMember : Entity
{
    /// <summary>
    /// GroupChat's unique identifier
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// User's unique identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Role of a user in the group chat
    /// <para>1 - member</para>
    /// <para>0 - admin</para>
    /// </summary>
    public int Role { get; set; }
}
