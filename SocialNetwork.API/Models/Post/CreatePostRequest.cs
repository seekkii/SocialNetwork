namespace SocialNetwork.API.Models.Post;

/// <summary>
/// Create new post, equivalent to post form in frontend
/// </summary>
public class CreatePostRequest
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
    /// Post's content
    /// </summary>
    public String Text { get; set; }

    /// <summary>
    /// Post's privacy
    /// <para>0 - public / public group</para>
    /// <para>1 - private / only me</para>
    /// <para>2 - group scoped (private group)</para>
    /// </summary>
    public int Privacy { get; set; }

    /// <summary>
    /// Post's check in location
    /// </summary>
    public String Location { get; set; }

    /// <summary>
    /// Media
    /// </summary>
    public IEnumerable<String> MediaPaths { get; set; }
}
