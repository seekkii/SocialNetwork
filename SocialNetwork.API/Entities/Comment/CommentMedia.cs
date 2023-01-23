namespace SocialNetwork.API.Entities.Comment;

/// <summary>
/// Comment's images, videos, ...
/// <para>Comment - 1 : n - CommentMedia</para>
/// </summary>
public class CommentMedia
{
    /// <summary>
    /// Media's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Media's comment's unique identifier
    /// <para>Reference to Comment.Id</para>
    /// </summary>
    public Guid CommentId { get; set; }

    /// <summary>
    /// Media path
    /// </summary>
    public String Path { get; set; }
}
