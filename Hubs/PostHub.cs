using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Comment;

namespace SocialNetwork.API.Hubs;

/// <summary>
/// Interface for strong-typed Hub (PostHub)
/// <para>Help prevent hard-coding event name sent to Front-end</para>
/// <para>by defining methods as the event name</para>
/// </summary>
public interface IPostHub
{
    /// <summary>
    /// Send "Reaction" event to Front-end whenever user likes/unlikes a post
    /// </summary>
    /// <param name="postId">Post's unique identifier</param>
    /// <param name="userId">Reacting user's unique identifier</param>
    /// <param name="like">True if like else false</param>
    /// <returns></returns>
    Task Reaction(Guid postId, Guid userId, bool like);

    /// <summary>
    /// Send "Reply" event to Front-end whenever user comments on a post
    /// </summary>
    /// <param name="comment">The commented comment</param>
    /// <returns></returns>
    Task Comment(Comment comment);
    
    /// <summary>
    /// Send "Edit" event to Front-end whenever user edit a comment
    /// </summary>
    /// <param name="comment">The edited comment</param>
    /// <returns></returns>
    Task Edit(Comment comment);
    
    /// <summary>
    /// Send "Delete" event to Front-end whenever user delete a comment
    /// </summary>
    /// <param name="comment">The deleted comment</param>
    /// <returns></returns>
    Task Delete(Guid id);

    /// <summary>
    /// Send "ViewingPost" event to Front-end whenever user click to view a post
    /// </summary>
    /// <param name="commentId">Post's unique identifier</param>
    /// <returns></returns>
    Task ViewingPost(Guid postId);

    /// <summary>
    /// Send "LeavingPost" event whenever user left (doesn't view anymore) that comment
    /// </summary>
    /// <param name="postId">Post's unique identifier</param>
    /// <returns></returns>
    Task LeavingPost(Guid postId);
}

/// <summary>
/// Strong-typed Hub
/// <para>Provides real-time interact with activities related to Comment in Front-end</para>
/// <para>
/// These async Tasks methods are called in API, then send corresponding event (defined same as methods name)
/// back to Front-end
/// </para>
/// </summary>
public class PostHub : Hub<IPostHub>
{
    #region Properties
    /// <summary>
    /// Log to API's console
    /// </summary>
    private readonly ILogger _logger;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Log to API's console</param>
    public PostHub(ILogger<PostHub> logger)
    {
        _logger = logger;
    }
    #endregion Constructor

    #region Methods
    public async Task ViewingPost(Guid postId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} is viewing {postId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, postId.ToString());
    }

    public async Task LeavingPost(Guid postId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} has left {postId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId.ToString());
    }
    #endregion Methods
}
