namespace SocialNetwork.API.Entities.User;

/// <summary>
/// Class stores following between users
/// </summary>
public class Follow : Entity
{
    /// <summary>
    /// Id of who follows
    /// </summary>
    public Guid FromId { get; set; }

    /// <summary>
    /// Id of who being followed
    /// </summary>
    public Guid ToId { get; set; }
}
