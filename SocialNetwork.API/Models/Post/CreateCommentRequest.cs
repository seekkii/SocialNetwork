namespace SocialNetwork.API.Models.Post;

/// <summary>
/// Create new comment, equivalent to comment form in frontend
/// </summary>
public class CreateCommentRequest
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
    /// If root comment -> Parent = Post
    /// <para>Reference to Post.Id</para>
    /// If node / leaf comment -> Parent = Comment
    /// <para>Reference to Comment.Id</para>
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// Comment's content
    /// </summary>
    public String Text { get; set; }

    /// <summary>
    /// Media
    /// </summary>
    public IEnumerable<String> MediaPaths { get; set; }
}
