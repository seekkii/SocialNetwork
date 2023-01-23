namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who saved which post
/// <para>User - n : n - Post -> Save</para>
/// </summary>
public class PostSave : Entity
{
    /// <summary>
    /// Saved post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Unique identifier of user who saved post 
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid UserId { get; set; }
}
