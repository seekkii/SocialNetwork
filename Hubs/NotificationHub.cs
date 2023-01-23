using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Entities.Notification;

namespace SocialNetwork.API.Hubs;

/// <summary>
/// Interface provides Tasks for NitificationHub
/// <para>Help prevent hard-coding event name sent to Front-end</para>
/// <para>by defining methods as the event name</para>
/// </summary>
public interface INotificationHub
{
    /// <summary>
    /// Send a notification to notification.ToId
    /// </summary>
    /// <param name="notification">An instance of a notification created alongside with an instance of an event</param>
    /// <returns></returns>
    Task Notify(Notification notification);

    Task Online(Guid userId);
}

/// <summary>
/// Strong-typed Hub
/// <para>Provides real-time interact with activities related to you in Front-end</para>
/// <para>Example: Like, share, follow, comment</para>
/// <para>
/// These async Tasks methods are called in API, then send corresponding event (defined same as methods name)
/// back to Front-end
/// </para>
/// </summary>
public class NotificationHub : Hub<INotificationHub>
{
    #region Properties
    /// <summary>
    /// Log to API's console
    /// </summary>
    private readonly ILogger _logger;
    private Dictionary<Guid, string> _users = new();
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Log to API's console</param>
    public NotificationHub(ILogger<PostHub> logger)
    {
        _logger = logger;
    }
    #endregion Constructor

    public async Task Notify(Notification notification)
    {
        var client = _users.Where(u => u.Key == notification.ToId);
        if (client.Any())
        {
            await Clients.Client(client.First().Value).Notify(notification);
        }
    }

    public async Task Online(Guid userId)
    {
        if (_users.ContainsKey(userId))
        {
            await Groups.RemoveFromGroupAsync(_users.First(u => u.Key == userId).Value, userId.ToString());
        }
        _users.Add(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        _logger.LogInformation($"User {userId} {Context.ConnectionId} is online.");
    }
}
