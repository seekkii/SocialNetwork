using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.Chat;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    #region Properties
    private readonly IChatService _chatService;
    #endregion Properties

    #region Constructor
    public ChatController(
        IChatService chatService
        )
    {
        _chatService = chatService;
    }
    #endregion Constructor

    #region Methods
    [HttpGet("user/{userId}")]
    public IActionResult GetChatList(Guid userId)
    {
        var chatList = _chatService.GetChatList(userId);
        return Ok(chatList);
    }

    [HttpGet("{fromId}/{toId}")]
    public IActionResult GetOneToOneChatId(Guid fromId, Guid toId)
    {
        var chatId = _chatService.GetOneToOneChatId(fromId, toId);
        return Ok(chatId);
    }

    [HttpPost("messages")]
    public IActionResult Send([FromBody] CreateMessageRequest model)
    {
        _chatService.Send(model);
        return Ok(new { Message = "Message sent" });
    }

    [HttpGet("{id}/name")]
    public IActionResult GetChatName(Guid id)
    {
        var chatName = _chatService.GetChatName(id);
        return Ok(chatName);
    }
    
    [HttpGet("{id}/{userId}/buddy")]
    public IActionResult GetOneToOneChatBuddyId(Guid id, Guid userId)
    {
        var buddy = _chatService.GetOneToOneChatBuddyId(id, userId);
        return Ok(buddy);
    }

    [HttpGet("{id}/history")]
    public IActionResult GetChatHistory(Guid id)
    {
        var chatHistory = _chatService.GetChatHistory(id);
        return Ok(chatHistory);
    }

    [HttpPost("{id}/messages/read/{userId}")]
    public IActionResult MarkAsRead(Guid id, Guid userId)
    {
        _chatService.MarkAsRead(id, userId);
        return Ok(new { Message = "Marked as read" });
    }

    [HttpDelete("messages/{id}")]
    public IActionResult DeleteMessage(Guid id)
    {
        _chatService.DeleteMessage(id);
        return Ok(new { Message = "Deleted" });
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteGroupChat(Guid id)
    {
        _chatService.DeleteChat(id);
        return Ok(new { Message = "Deleted" });
    }
    #endregion Methods
}
