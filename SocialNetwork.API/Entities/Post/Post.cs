namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Post class
/// </summary>
public class Post : Entity
{
    /// <summary>
    /// Post's author's unique identifier
    /// <para>Reference to User.Id</para>
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Group's unique identifier if post is posted in a group
    /// <para>Default</para>
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// Post's privacy
    /// <para>0 - public / public group</para>
    /// <para>1 - private / only me</para>
    /// <para>2 - group scoped (private group)</para>
    /// </summary>
    public int Privacy { get; set; }

    /// <summary>
    /// Post's text content
    /// </summary>
    public String Text { get; set; }

    /// <summary>
    /// Post's check in location
    /// </summary>
    public String Location { get; set; }
}
