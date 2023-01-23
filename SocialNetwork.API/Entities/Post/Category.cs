namespace SocialNetwork.API.Entities.Post;

/// <summary>
/// Category class
/// </summary>
public class Category
{
    /// <summary>
    /// Category's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Category's name
    /// </summary>
    public String Name { get; set; }
}
