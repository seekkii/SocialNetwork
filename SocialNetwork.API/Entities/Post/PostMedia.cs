namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Post's images, videos, ...
/// <para>Post - 1 : n - PostMedia</para>
/// </summary>
public class PostMedia
{
    /// <summary>
    /// Media's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Media's post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Media path
    /// </summary>
    public String Path { get; set; }
}
