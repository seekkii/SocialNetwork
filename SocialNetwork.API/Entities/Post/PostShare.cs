namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Who shared which post
/// <para>User - n : n : Post -> PostShare</para>
/// </summary>
public class PostShare : Entity
{
    /// <summary>
    /// Post's unique identifier
    /// <para>Reference to Post.Id</para>
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Who shared the entity
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid FromId { get; set; }

    /// <summary>
    /// Who (User) / Which (Group) received the shared post
    /// <para>Reference to User.Id / Group.Id</para>
    /// </summary>
    public Guid ToId { get; set; }
}
