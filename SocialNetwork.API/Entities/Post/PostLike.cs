namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who liked which post
/// <para>User - n : n : Post -> PostLike</para>
/// </summary>
public class PostLike : Entity
{
    /// <summary>
    /// Post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }
}
