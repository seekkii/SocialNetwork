namespace SocialNetwork.API.Entities.Chat;

/// <summary>
/// Class for group chat
/// </summary>
public class Chat : Entity
{
    /// <summary>
    /// Name of the chat
    /// <para>For group chat only</para>
    /// </summary>
    public String Name { get; set; }
}
