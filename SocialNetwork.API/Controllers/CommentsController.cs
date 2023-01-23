using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.Post;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to Comment
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    #region Properties
    /// <summary>
    /// Provide queries of Comment table to DB
    /// </summary>
    private readonly ICommentService _commentService;
    
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="commentService"></param>
    public CommentsController(
        ICommentService commentService)
    {
        _commentService = commentService;
    }
    #endregion Constructor

    #region Methods

    /// <summary>
    /// Get a comment by its id
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>A comment with provided id</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var comment = _commentService.GetById(id);
        return Ok(comment);
    }

    /// <summary>
    /// Get all likes for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List of likes this comment</returns>
    [HttpGet("{id}/likes")]
    public IActionResult GetAllLikesByCommentId(Guid id)
    {
        var likes = _commentService.GetAllLikesByCommentId(id);
        return Ok(likes);
    }

    /// <summary>
    /// Check if this auth user liked this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    [HttpGet("{id}/likes/{userId}")]
    public IActionResult IsAuthUserLiked(Guid id, Guid userId)
    {
        var liked = _commentService.IsAuthUserLiked(id, userId);
        return Ok(liked);
    }

    /// <summary>
    /// Auth user likes this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    [HttpPost("{id}/likes/{userId}/like")]
    public IActionResult Like(Guid id, Guid userId)
    {
        _commentService.Like(id, userId);
        return Ok(new { Message = "Liked" });
    }

    /// <summary>
    /// Auth user unliked this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    [HttpPost("{id}/likes/{userId}/unlike")]
    public IActionResult Unlike(Guid id, Guid userId)
    {
        _commentService.Unlike(id, userId);
        return Ok(new { Message = "Unliked" });
    }

    /// <summary>
    /// Get all children for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List all chidren for this comment</returns>
    [HttpGet("{id}/comments")]
    public IActionResult GetAllChildrenByCommentId(Guid id)
    {
        var comments = _commentService.GetAllChildrenByCommentId(id);
        return Ok(comments);
    }

    /// <summary>
    /// Get number of children for this comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List number of children for this comment</returns>
    [HttpGet("{id}/comments/count")]
    public IActionResult GetChildCountByCommentId(Guid id)
    {
        var numberOfComments = _commentService.GetAllChildrenByCommentId(id).Count();
        return Ok(numberOfComments);
    }

    /// <summary>
    /// Get comment's media
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>List of media attached to this comment</returns>
    [HttpGet("{id}/media")]
    public IActionResult GetMedia(Guid id)
    {
        var media = _commentService.GetMedia(id);
        return Ok(media);
    }

    /// <summary>
    /// Create new comment
    /// </summary>
    /// <param name="model">Fields to create a comment</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost]
    public IActionResult Create(CreateCommentRequest model)
    {
        _commentService.Create(model);
        return Ok();
    }

    /// <summary>
    /// Edit a comment
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <param name="model">Comment's new information</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}")]
    public IActionResult Edit(Guid id, CreateCommentRequest model)
    {
        _commentService.Edit(id, model);
        return Ok();
    }

    /// <summary>
    /// Delete a comment by its id
    /// </summary>
    /// <param name="id">Comment's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _commentService.Delete(id);
        return Ok();
    }

    #endregion Methods
}

