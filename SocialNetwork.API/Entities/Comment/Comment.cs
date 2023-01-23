namespace SocialNetwork.API.Entities.Comment;

/// <summary>
/// Comment class
/// <para>Post - 1 : n - Comment</para>
/// <para>Comment - 1 : n - Comment</para>
/// </summary>
public class Comment : Entity
{
    /// <summary>
    /// Comment's author's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Post contains this comment
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// If node / leaf comment -> Parent = Comment
    /// <para>Reference to Comment.Id</para>
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// Comment's content
    /// </summary>
    public String Text { get; set; }
}
