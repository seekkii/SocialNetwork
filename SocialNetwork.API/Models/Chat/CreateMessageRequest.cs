namespace SocialNetwork.API.Models.Chat;

public class CreateMessageRequest
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

    public IEnumerable<String> MediaPaths { get; set; }
}
