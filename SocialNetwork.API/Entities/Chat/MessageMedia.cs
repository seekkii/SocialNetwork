namespace SocialNetwork.API.Entities.Chat;

/// <summary>
/// Stores media of message
/// </summary>
public class MessageMedia
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Message's unique identifier
    /// </summary>
    public Guid MessageId { get; set; }

    /// <summary>
    /// Path to the media file
    /// </summary>
    public String Path { get; set; }
}
