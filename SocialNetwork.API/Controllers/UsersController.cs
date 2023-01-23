using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Models.User;
using SocialNetwork.API.Services;

namespace SocialNetwork.API.Controllers;

/// <summary>
/// Provides API related to User
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    #region Properties
    /// <summary>
    /// Provide queries of User table to DB
    /// </summary>
    private readonly IUserService _userService;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userService"></param>
    public UsersController(
        IUserService userService)
    {
        _userService = userService;
    }
    #endregion Constructor

    #region Methods
    /// <summary>
    /// Log user in
    /// </summary>
    /// <param name="model">Auth model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data:
    /// <para>User's credentials and Jwt token if success</para>
    /// </returns>
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }

    /// <summary>
    /// Register new user account
    /// </summary>
    /// <param name="model">Register model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: None
    /// </returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest model)
    {
        _userService.Register(model);
        return Ok(new { message = "Registration successful" });
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: List of all users in DB
    /// </returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    /// <summary>
    /// Get a user by id
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: One user
    /// </returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    /// <summary>
    /// Update a user's credentials
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <param name="model">Credentials update model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: None
    /// </returns>
    [HttpPut("{id}/credentials")]
    public IActionResult UpdateCredentials(Guid id, UpdateCredentialsRequest model)
    {
        _userService.UpdateCredentials(id, model);
        return Ok(new { message = "User updated successfully" });
    }

    /// <summary>
    /// Delete a user by his/her id
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// Data: None
    /// </returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted successfully" });
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>User profile</returns>
    [HttpGet("{id}/profile")]
    public IActionResult GetProfile(Guid id)
    {
        var profile = _userService.GetProfile(id);
        return Ok(profile);
    }

    /// <summary>
    /// Get only avatar url and name
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Avatar and name</returns>
    [HttpGet("{id}/profile/avatarname")]
    public IActionResult GetAvatarName(Guid id)
    {
        var avatarName = _userService.GetAvatarName(id);
        return Ok(avatarName);
    }
    
    /// <summary>
    /// Get only avatar url
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Avatar</returns>
    [HttpGet("{id}/profile/avatar")]
    public IActionResult GetAvatar(Guid id)
    {
        var avatarName = _userService.GetAvatar(id);
        return Ok(avatarName);
    }
    
    /// <summary>
    /// Get only name
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>Name</returns>
    [HttpGet("{id}/profile/name")]
    public IActionResult GetName(Guid id)
    {
        var avatarName = _userService.GetName(id);
        return Ok(avatarName);
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="id">User's unque identifier</param>
    /// <param name="model">Update profile model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}/profile")]
    public IActionResult UpdateProfile(Guid id, [FromBody] UpdateProfileRequest model)
    {
        _userService.UpdateProfile(id, model);
        return Ok(new { message = "Profile updated successfully!" });
    }

    /// <summary>
    /// Update user settings
    /// </summary>
    /// <param name="id">User's unque identifier</param>
    /// <param name="model">Update setting model</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPatch("{id}/setting")]
    public IActionResult UpdateSetting(Guid id, UpdateSettingRequest model)
    {
        _userService.UpdateSetting(id, model);
        return Ok(new { message = "Settings updated successfully!" });
    }

    /// <summary>
    /// Get all posts posted/shared by this user
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of all posts posted/shared by this user</returns>
    [HttpGet("{id}/posts")]
    public IActionResult GetAllPostsByUserId(Guid id)
    {
        var posts = _userService.GetAllPostsByUserId(id);
        return Ok(posts);
    }

    /// <summary>
    /// Get all posts saved by this user
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of all posts saved by this user</returns>
    [HttpGet("{id}/posts/saved")]
    public IActionResult GetAllSavedPostsByUserId(Guid id)
    {
        var savedPosts = _userService.GetAllSavedPostsByUserId(id);
        return Ok(savedPosts);
    }

    /// <summary>
    /// Populate user's newsfeed
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of posts to display in user's newsfeed</returns>
    [HttpGet("{id}/feed")]
    public IActionResult GetFeed(Guid id)
    {
        var feed = _userService.GetFeed(id);
        return Ok(feed);
    }

    /// <summary>
    /// fromId follows toId
    /// </summary>
    /// <param name="fromId">Id of who follows</param>
    /// <param name="toId">Id of who being followed</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost("{fromId}/follow/{toId}")]
    public IActionResult Follow(Guid fromId, Guid toId)
    {
        _userService.Follow(fromId, toId);
        return Ok(new { Message = "Followed" });
    }
    
    /// <summary>
    /// fromId unfollows toId
    /// </summary>
    /// <param name="fromId">Id of who unfollows</param>
    /// <param name="toId">Id of who being unfollowed</param>
    /// <returns>Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpPost("{fromId}/unfollow/{toId}")]
    public IActionResult Unfollow(Guid fromId, Guid toId)
    {
        _userService.Unfollow(fromId, toId);
        return Ok(new { Message = "Unfollowed" });
    }

    /// <summary>
    /// Check if this user has followed the being viewed user (on UserProfile)
    /// </summary>
    /// <param name="fromId">This auth user's unique identifier</param>
    /// <param name="toId">The being viewed user's unique identifier</param>
    /// <returns>
    /// Status code:
    /// <para>200 if success, otherwise failed</para>
    /// </returns>
    [HttpGet("{fromId}/follow/{toId}")]
    public IActionResult HasAuthUserFollowed(Guid fromId, Guid toId)
    {
        var followed = _userService.HasAuthUserFollowed(fromId, toId);
        return Ok(followed);
    }

    /// <summary>
    /// Get the list of who followed the user with id
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of Guid of who followed</returns>
    [HttpGet("{id}/followers")]
    public IActionResult GetFollowers(Guid id)
    {
        var followers = _userService.GetFollowers(id);
        return Ok(followers);
    }

    /// <summary>
    /// Get the number of followers
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>The number of followers</returns>
    [HttpGet("{id}/followers/count")]
    public IActionResult GetFollowerCount(Guid id)
    {
        var followerCount = _userService.GetFollowers(id).Count();
        return Ok(followerCount);
    }

    /// <summary>
    /// Get the list of who the user with id is following
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>List of Guid</returns>
    [HttpGet("{id}/followees")]
    public IActionResult GetFollowees(Guid id)
    {
        var followers = _userService.GetFollowees(id);
        return Ok(followers);
    }

    /// <summary>
    /// Get the number of followees
    /// </summary>
    /// <param name="id">User's unique identifier</param>
    /// <returns>The number of followees</returns>
    [HttpGet("{id}/followees/count")]
    public IActionResult GetFolloweeCount(Guid id)
    {
        var followerCount = _userService.GetFollowees(id).Count();
        return Ok(followerCount);
    }
    #endregion Methods
}
