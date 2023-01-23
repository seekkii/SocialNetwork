using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Chat;

namespace SocialNetwork.API.Hubs;

public interface IChatHub
{
    Task Send(Message message);

    Task Delete(Guid messageId);
}
public class ChatHub : Hub<IChatHub>
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
    public ChatHub(ILogger<CommentHub> logger)
    {
        _logger = logger;
    }
    #endregion Constructor

    #region Methods

    public async Task ViewingChat(Guid chatId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} is viewing {chatId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeavingChat(Guid chatId)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} has left {chatId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    #endregion Methods
}
