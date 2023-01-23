using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.Post;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to Post
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    #region Properties
    /// <summary>
    /// Provide queries of Post table to DB
    /// </summary>
    private readonly IPostService _postService;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="postService"></param>
    public PostsController(
        IPostService postService)
    {
        _postService = postService;
    }
    #endregion Constructor

    #region Methods

    /// <summary>
    /// Get a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>A post with provided id</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var post = _postService.GetById(id);
        return Ok(post);
    }

    /// <summary>
    /// Get all likes for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of likes this post</returns>
    [HttpGet("{id}/likes")]
    public IActionResult GetAllLikesByPostId(Guid id)
    {
        var likes = _postService.GetAllLikesByPostId(id);
        return Ok(likes);
    }

    /// <summary>
    /// Get all comments' Id for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List all comments' Id for this post</returns>
    [HttpGet("{id}/comments")]
    public IActionResult GetAllCommentsByPostId(Guid id)
    {
        var comments = _postService.GetAllCommentsByPostId(id);
        return Ok(comments);
    }

    /// <summary>
    /// Get number of comments for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List number of comments for this post</returns>
    [HttpGet("{id}/comments/count")]
    public IActionResult GetCommentCountByPostId(Guid id)
    {
        var numberOfComments = _postService.GetAllCommentsByPostId(id).Count();
        return Ok(numberOfComments);
    }

    /// <summary>
    /// Check if this auth user liked this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    [HttpGet("{id}/likes/{userId}")]
    public IActionResult HasAuthUserLiked(Guid id, Guid userId)
    {
        var liked = _postService.HasAuthUserLiked(id, userId);
        return Ok(liked);
    }

    /// <summary>
    /// Auth user likes this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost("{id}/likes/{userId}/like")]
    public IActionResult Like(Guid id, Guid userId)
    {
        _postService.Like(id, userId);
        return Ok(new { Message = "Liked" });
    }

    /// <summary>
    /// Auth user unliked this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost("{id}/likes/{userId}/unlike")]
    public IActionResult Unlike(Guid id, Guid userId)
    {
        _postService.Unlike(id, userId);
        return Ok(new { Message = "Unliked" });
    }

    /// <summary>
    /// Get all shares for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List all shares for this post</returns>
    [HttpGet("{id}/shares")]
    public IActionResult GetAllSharesByPostId(Guid id)
    {
        var shares = _postService.GetAllSharesByPostId(id);
        return Ok(shares);
    }

    /// <summary>
    /// Get post's media
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of media attached to this post</returns>
    [HttpGet("{id}/media")]
    public IActionResult GetMedia(Guid id)
    {
        var media = _postService.GetMedia(id);
        return Ok(media);
    }

    [HttpDelete("media/{mediaPath}/delete")]
    public IActionResult DeleteMedia(String mediaPath)
    {
        _postService.DeleteMedia(mediaPath);
        return Ok(new { Message = "Media deleted" });
    }

    /// <summary>
    /// Save a post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns></returns>
    [HttpPost("{id}/save/{userId}")]
    public IActionResult Save(Guid id, Guid userId)
    {
        _postService.Save(id, userId);
        return Ok(new { Message = "Post saved" });
    }

    /// <summary>
    /// Create new post
    /// </summary>
    /// <param name="model">Fields to create a post</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost]
    public IActionResult Create([FromBody] CreatePostRequest model)
    {
        _postService.Create(model);
        return Ok(new { Message = "Posted successfully" });
    }

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="model">Post's new information</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}")]
    public IActionResult Edit(Guid id, [FromBody] CreatePostRequest model)
    {
        _postService.Edit(id, model);
        return Ok();
    }

    /// <summary>
    /// Delete a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _postService.Delete(id);
        return Ok();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {

        //var path = _postService.Upload(HttpContext.Request.Form.Files[0]);

        IFormFile file = HttpContext.Request.Form.Files[0];
        var path = "D:\\Code\\Web\\Backend\\SocialNetwork\\SocialNetwork.UI\\src\\assets\\";
        var fileExtension = "." + file.FileName.Split(".").Last();
        Console.Write(fileExtension);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var destPath = path + (Guid.NewGuid().ToString() + fileExtension);

        using (var stream = System.IO.File.Create(destPath))
        {
            await file.CopyToAsync(stream);
        }
        destPath = destPath.Replace(path, "");

        return Ok(destPath);
    }

    [HttpPost("unupload")]
    public IActionResult Unupload()
    {
        var src = HttpContext.Request.Form.Keys.First().ToString().Replace("blob:http://localhost:8080/", "") + ".jpg";
        var path = "E:\\Code\\Web\\Backend\\SocialNetwork\\SocialNetwork.UI\\src\\assets\\" + src;
        System.IO.File.Delete(path);
        return Ok();
    }

    #endregion Methods
}
