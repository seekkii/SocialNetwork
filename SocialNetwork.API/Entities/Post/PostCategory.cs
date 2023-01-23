namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Post's categories
/// <para>Post - 1 : n - Category -> PostCategory</para>
/// </summary>
public class PostCategory
{
    /// <summary>
    /// Post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Category's unique identifier
    /// <para>Reference to Category.Id</para>
    /// </summary>
    public Guid CategoryId { get; set; }
}
