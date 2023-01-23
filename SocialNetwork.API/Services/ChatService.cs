using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.API.Entities.Chat;
using SocialNetwork.API.Entities.Notification;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Hubs;
using SocialNetwork.API.Models.Chat;

namespace SocialNetwork.API.Services;

public interface IChatService
{
    void CreateChat(Guid userId);

    void AddChatMember(Guid chatId, Guid userId);

    Guid GetOneToOneChatId(Guid fromId, Guid toId);

    string GetChatName(Guid id);

    Guid GetOneToOneChatBuddyId(Guid id, Guid userId);

    void Send(CreateMessageRequest model);

    IEnumerable<Message> GetChatHistory(Guid id);

    void MarkAsRead(Guid id, Guid userId);

    void DeleteMessage(Guid id);
    void DeleteChat(Guid id);

    IEnumerable<Message> GetChatList(Guid userId);
    //TODO: Add methods
}

public class ChatService : IChatService
{
    #region Properties
    private DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatHub> _chatHub;
    private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
    #endregion Properties

    #region Constructor
    public ChatService(
        DataContext context,
        IMapper mapper,
        IHubContext<ChatHub, IChatHub> chatHub,
        IHubContext<NotificationHub, INotificationHub> notificationHub)
    {
        _context = context;
        _mapper = mapper;
        _chatHub = chatHub;
        _notificationHub = notificationHub;
    }
    #endregion Constructor

    #region Methods
    public void CreateChat(Guid userId)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Name = "",
            Timestamp = DateTime.Now
        };
        var chatCreator = new ChatMember
        {
            Id = Guid.NewGuid(),
            ChatId = chat.Id,
            UserId = userId,
            Role = 0,
            Timestamp = DateTime.Now
        };
        _context.Chat.Add(chat);
        _context.ChatMember.Add(chatCreator);
        _context.SaveChanges();
    }

    public void AddChatMember(Guid chatId, Guid userId)
    {
        var chatMember = new ChatMember
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            UserId = userId,
            Role = 1,
            Timestamp = DateTime.Now
        };
        _context.ChatMember.Add(chatMember);
        _context.SaveChanges();
    }

    public Guid GetOneToOneChatId(Guid fromId, Guid toId)
    {
        var chatsOfFromId = _context.ChatMember
            .Where(cm => cm.UserId == fromId).Select(cm => cm.ChatId).ToList();
        var chatsOfToId = _context.ChatMember
            .Where(cm => cm.UserId == toId).Select(cm => cm.ChatId).ToList();
        var commonChatIds = chatsOfFromId.Intersect(chatsOfToId);

        foreach (var commonChatId in commonChatIds)
        {
            var memberCount = _context.ChatMember
                .Where(cm => cm.ChatId == commonChatId).Count();
            if (memberCount == 2)
            {
                return commonChatId;
            }
        }

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Name = "",
            Timestamp = DateTime.Now
        };
        var chatMem1 = new ChatMember
        {
            ChatId = chat.Id,
            UserId = fromId,
            Role = 0,
            Timestamp = DateTime.Now
        };
        var chatMem2 = new ChatMember
        {
            ChatId = chat.Id,
            UserId = toId,
            Role = 0,
            Timestamp = DateTime.Now
        };
        _context.Chat.Add(chat);
        _context.ChatMember.Add(chatMem1);
        _context.ChatMember.Add(chatMem2);
        _context.SaveChanges();

        return chat.Id;
    }

    public string GetChatName(Guid id)
    {
        return _context.Chat.Find(id).Name;
    }

    public Guid GetOneToOneChatBuddyId(Guid id, Guid userId)
    {
        var buddyId = _context.ChatMember
            .Where(cm => cm.ChatId == id)
            .Where(cm => cm.UserId != userId)
            .SingleOrDefault().UserId;
        return buddyId;
    }

    public void Send(CreateMessageRequest model)
    {
        var message = _mapper.Map<Message>(model);
        message.Timestamp = DateTime.Now;
        _context.Message.Add(message);

        if (model.MediaPaths.Any())
        {
            foreach (var mediaPath in model.MediaPaths)
            {
                var messageMedia = new MessageMedia
                {
                    Id = Guid.NewGuid(),
                    MessageId = message.Id,
                    Path = mediaPath
                };
                _context.MessageMedia.Add(messageMedia);
            }
        }

        var newMsgNotification = new Notification
        {
            FromId = message.UserId,
            ToId = message.ChatId,
            Verb = $"{_context.UserProfile.First(up => up.UserId == message.UserId).Name} has sent a message",
            EntityId = message.Id,
            Read = false,
            Timestamp = DateTime.Now
        };
        var chatMembers = _context.ChatMember
            .Where(cm => cm.ChatId == message.ChatId)
            .Where(cm => cm.UserId != message.UserId)
            .Select(cm => cm.UserId)
            .ToList();

        _chatHub.Clients.Group(message.ChatId.ToString()).Send(message);
        foreach (var chatMember in chatMembers)
        {
            _notificationHub.Clients.Group(chatMember.ToString()).Notify(newMsgNotification);
        }
        _context.SaveChanges();
    }

    public IEnumerable<Message> GetChatHistory(Guid id)
    {
        return _context.Message
            .Where(m => m.ChatId == id)
            .ToList();
    }

    public void MarkAsRead(Guid id, Guid userId)
    {
        var msgs = _context.Message
            .Where(m => m.ChatId == id)
            .Where(m => m.UserId != userId)
            .Where(m => m.Read == false)
            .ToList();
        foreach (var msg in msgs)
        {
            msg.Read = true;
        }
        _context.SaveChanges();
    }

    public void DeleteMessage(Guid id)
    {
        var message = _context.Message.Find(id);
        _context.Message.Remove(message);

        _context.MessageMedia.RemoveRange(_context.MessageMedia.Where(m => m.MessageId == id).ToList());

        _chatHub.Clients.Group(message.ChatId.ToString()).Delete(message.Id);
        _context.SaveChanges();
    }
    
    public void DeleteChat(Guid id)
    {
        var chat = _context.Chat.Find(id);
        _context.Chat.Remove(chat);

        _context.SaveChanges();
    }

    public IEnumerable<Message> GetChatList(Guid userId)
    {
        var myChatIds = _context.ChatMember
            .Where(cm => cm.UserId == userId)
            .Select(cm => cm.ChatId).ToList();

        var msgList = new List<Message>();
        foreach (var myChatId in myChatIds)
        {
            var latestMsg = _context.Message
                .Where(m => m.ChatId == myChatId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefault();
            if (latestMsg != null)
            {
                msgList.Add(latestMsg);
            }
        }
        return msgList;
    }
    #endregion Methods
}
