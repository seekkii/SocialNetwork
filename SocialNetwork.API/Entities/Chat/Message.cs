namespace SocialNetwork.API.Entities.Chat;

/// <summary>
/// Message from a user to a user or to a group chat
/// </summary>
public class Message : Entity
{
    /// <summary>
    /// User who sent message unique identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Chat's unique identifier
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// Message's parent if this is a reply message
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Text of message
    /// </summary>
    public String Text { get; set; }

    /// <summary>
    /// Whether message is read or not
    /// <para>0 - unread</para>
    /// <para>1 - read</para>
    /// </summary>
    public Boolean Read { get; set; } = false;
}
