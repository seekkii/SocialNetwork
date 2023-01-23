using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Helpers;

namespace SocialNetwork.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EntityController : ControllerBase
{
    private readonly DataContext _context;

    public EntityController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public IActionResult GetEntityType(Guid id)
    {
        var postEntity = _context.Post.Find(id);
        if (postEntity != null)
        {
            return Ok("post");
        }

        var commentEntity = _context.Comment.Find(id);
        if (commentEntity != null)
        {
            return Ok("post");
        }

        var userEntity = _context.User.Find(id);
        if (userEntity != null)
        {
            return Ok("user");
        }

        return BadRequest();
    }
}
