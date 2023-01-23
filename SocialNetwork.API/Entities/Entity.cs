namespace SocialNetwork.API.Entities;

/// <summary>
/// Base entity
/// </summary>
public class Entity
{
    /// <summary>
    /// Entity's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Entity created timestamp
    /// </summary>
    public DateTime? Timestamp { get; set; }
}
