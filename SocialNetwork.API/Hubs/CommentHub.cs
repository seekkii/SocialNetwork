using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Comment;

namespace SocialNetwork.API.Hubs;

/// <summary>
/// Interface for strong-typed Hub (CommentHub)
/// <para>Help prevent hard-coding event name sent to Front-end</para>
/// <para>by defining methods as the event name</para>
/// </summary>
public interface ICommentHub
{
    /// <summary>
    /// Send "Reaction" event to Front-end whenever user likes/unlikes a comment
    /// </summary>
    /// <param name="commentId">Comment's unique identifier</param>
    /// <param name="userId">Reacting user's unique identifier</param>
    /// <param name="like">True if like else false</param>
    /// <returns></returns>
    Task Reaction(Guid commentId, Guid userId, bool like);

    /// <summary>
    /// Send "Reply" event to Front-end whenever use replies on a comment
    /// <para>Not being implemented yet</para>
    /// </summary>
    /// <param name="comment">The replied comment</param>
    /// <returns></returns>
    Task Reply(Comment comment);

    /// <summary>
    /// Send "ViewingComment" event to Front-end whenever user click to view a comment
    /// </summary>
    /// <param name="commentId">Comment's unique identifier</param>
    /// <returns></returns>
    Task ViewingComment(Guid commentId);

    /// <summary>
    /// Send "LeavingComment" event whenever user left (doesn't view anymore) that comment
    /// </summary>
    /// <param name="commentId">Comment's unique identifier</param>
    /// <returns></returns>
    Task LeavingComment(Guid commentId);
}

/// <summary>
/// Strong-typed Hub
/// <para>Provides real-time interact with activities related to Comment in Front-end</para>
/// <para>
/// These async Tasks methods are called in API, then send corresponding event (defined same as methods name)
/// back to Front-end
/// </para>
/// </summary>
public class CommentHub : Hub<ICommentHub>
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
    public CommentHub(ILogger<CommentHub> logger)
    {
        _logger = logger;
    }
    #endregion Constructor

    #region Methods
    public async Task Reaction(Guid commentId, Guid userId, bool like)
    {
        await Clients.Group(commentId.ToString()).Reaction(commentId, userId, like);
    }

    public async Task Reply(Comment comment)
    {
        await Clients.Group(comment.ParentId.ToString()).Reply(comment);
    }

    public async Task ViewingComment(Guid commentId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} is viewing {commentId}");
    }
    #endregion Methods
}
