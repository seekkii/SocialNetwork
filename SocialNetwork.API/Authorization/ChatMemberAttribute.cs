using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.API.Entities.User;
using SocialNetwork.API.Helpers;

namespace SocialNetwork.API.Authorization;


public class ChatMemberAttribute : ActionFilterAttribute
{
    private DataContext _context;

    public ChatMemberAttribute(DataContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey("id"))
        {
            var chatId = context.ActionArguments["id"];
            var chatMemberId = _context.Post.Find(chatId).AuthorId;
            var userId = ((User)context.HttpContext.Items["User"]).Id;

            if (userId != chatMemberId)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
