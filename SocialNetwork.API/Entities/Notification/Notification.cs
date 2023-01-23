namespace SocialNetwork.API.Entities.Notification;

/// <summary>
/// Class stores notification to users
/// <para>Formula: FromId (has) Verb (on) you(r)(ToId) EntityId (on) Timestamp</para>
/// <para>Example: Long has commented on your post "Hi everyone..." 2 minutes ago</para>
/// </summary>
public class Notification : Entity
{
    /// <summary>
    /// Id of who triggered the event
    /// </summary>
    public Guid FromId { get; set; }

    /// <summary>
    /// Id of who receives the event
    /// </summary>
    public Guid ToId { get; set; }

    /// <summary>
    /// The event
    /// </summary>
    public String Verb { get; set; }

    /// <summary>
    /// Id of which being targeted in the event
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Watch status of has been read or not
    /// </summary>
    public bool Read { get; set; } = false;
}
