namespace SocialNetwork.API.Entities.Comment;

/// <summary>
/// Who liked which comment
/// <para>User - n : n : Comment -> CommentLike</para>
/// </summary>
public class CommentLike : Entity
{
    /// <summary>
    /// Comment's unique identifier
    /// <para>Reference to Comment.Id</para>
    /// </summary>
    public Guid CommentId { get; set; }

    /// <summary>
    /// User's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }
}
