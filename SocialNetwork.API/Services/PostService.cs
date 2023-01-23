using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Entities.Comment;
using SocialNetwork.API.Entities.Notification;
using SocialNetwork.API.Entities.Post;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Hubs;
using SocialNetwork.API.Models.Post;

namespace SocialNetwork.API.Services;

/// <summary>
/// Interface provides methods to PostService class
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Get a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>A post with provided id</returns>
    Post GetById(Guid id);

    /// <summary>
    /// Get all comments of this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of comments of this post</returns>
    IEnumerable<Guid> GetAllCommentsByPostId(Guid id);

    /// <summary>
    /// Get all likes for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of likes for this post</returns>
    IEnumerable<PostLike> GetAllLikesByPostId(Guid id);

    /// <summary>
    /// Check if this auth user liked this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    /// <returns>True if liked otherwise false</returns>
    bool HasAuthUserLiked(Guid id, Guid userId);

    /// <summary>
    /// Auth user likes this post
    /// <para>This method has 2 signalr hubs injected: PostHub, NotificationHub</para>
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    void Like(Guid id, Guid userId);

    /// <summary>
    /// Auth user unliked this post
    /// <para>This method has 1 signalr hubs injected: PostHub</para>
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    void Unlike(Guid id, Guid userId);

    /// <summary>
    /// Get all shares for this post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List all shares for this post</returns>
    IEnumerable<PostShare> GetAllSharesByPostId(Guid id);

    /// <summary>
    /// Get post's media
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <returns>List of media attached to this post</returns>
    IEnumerable<String> GetMedia(Guid id);

    /// <summary>
    /// Delete a post media
    /// </summary>
    /// <param name="mediaPath"></param>
    void DeleteMedia(String mediaPath);

    /// <summary>
    /// Save a post
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    /// <param name="userId">User's unique identifier</param>
    void Save(Guid id, Guid userId);

    /// <summary>
    /// Create new post
    /// </summary>
    /// <param name="model">Required fields to create new post form</param>
    void Create(CreatePostRequest model);

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="model">Required fields to edit a post form</param>
    void Edit(Guid id, CreatePostRequest model);

    /// <summary>
    /// Delete a post by its id
    /// </summary>
    /// <param name="id">Post's unique identifier</param>
    void Delete(Guid id);
}

/// <summary>
/// Provides queries and helpers of Post table to DB
/// </summary>
public class PostService : IPostService
{
    #region Properties
    private DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<PostHub, IPostHub> _postHubContext;
    private readonly IHubContext<NotificationHub, INotificationHub> _notificationHubContext;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public PostService(
        DataContext context,
        IMapper mapper,
        IHubContext<PostHub, IPostHub> postHubContext,
        IHubContext<NotificationHub, INotificationHub> notificationHubContext)
    {
        _context = context;
        _mapper = mapper;
        _postHubContext = postHubContext;
        _notificationHubContext = notificationHubContext;
    }
    #endregion Constructor

    #region Methods
    public Post GetById(Guid id)
    {
        var post = _context.Post.Find(id);
        return post;
    }

    public IEnumerable<Guid> GetAllCommentsByPostId(Guid id)
    {
        var comments = _context.Comment
            .Where(c => c.PostId == id)
            .OrderBy(c => c.Timestamp)
            .Reverse()
            .Select(c => c.Id).ToList();
        return comments;
    }

    public IEnumerable<PostLike> GetAllLikesByPostId(Guid id)
    {
        var likes = _context.PostLike
            .Where(l => l.PostId == id).ToList();
        return likes;
    }

    public IEnumerable<PostShare> GetAllSharesByPostId(Guid id)
    {
        var shares = _context.PostShare
            .Where(s => s.PostId == id).ToList();
        return shares;
    }

    public bool HasAuthUserLiked(Guid id, Guid userId)
    {
        return _context.PostLike
            .Where(l => l.PostId == id)
            .Any(l => l.UserId == userId);
    }

    public void Like(Guid id, Guid userId)
    {
        var like = new PostLike
        {
            PostId = id,
            UserId = userId,
            Timestamp = DateTime.Now,
        };
        _context.PostLike.Add(like);

        var notifyToId = _context.Post.Find(id).AuthorId;
        var likeNotification = new Notification
        {
            FromId = userId,
            ToId = notifyToId,
            Verb = $"{_context.UserProfile.First(up => up.UserId == userId).Name} liked your post \"{_context.Post.Find(id).Text}\"",
            EntityId = id,
            Read = false,
            Timestamp = DateTime.Now
        };
        //_context.Notification.Add(likeNotification);

        _notificationHubContext.Clients.Group(notifyToId.ToString()).Notify(likeNotification);
        _postHubContext.Clients.All.Reaction(id, userId, true);

        _context.SaveChanges();
    }

    public void Unlike(Guid id, Guid userId)
    {
        var like = _context.PostLike
            .Where(l => l.PostId == id)
            .Single(l => l.UserId == userId);
        _context.PostLike.Remove(like);
        _context.SaveChanges();

        _postHubContext.Clients.All.Reaction(id, userId, false);
    }

    public IEnumerable<String> GetMedia(Guid id)
    {
        var mediaPaths = _context.PostMedia
            .Where(pm => pm.PostId == id)
            .Select(pm => pm.Path);
        return mediaPaths;
    }

    public void DeleteMedia(String mediaPath)
    {
        var media = _context.PostMedia
            .First(pm => pm.Path == mediaPath);
        _context.PostMedia.Remove(media);
        _context.SaveChanges();
    }

    public void Save(Guid id, Guid userId)
    {
        var postSaved = new PostSave
        {
            Id = Guid.NewGuid(),
            PostId = id,
            UserId = userId,
            Timestamp = DateTime.Now
        };
        _context.PostSave.Add(postSaved);
        _context.SaveChanges();
    }

    public void Create(CreatePostRequest model)
    {
        var post = _mapper.Map<Post>(model);
        post.Id = Guid.NewGuid();
        post.Timestamp = DateTime.Now;
        _context.Post.Add(post);

        if (model.MediaPaths.Any())
        {
            foreach (var mediaPath in model.MediaPaths)
            {
                var postMedia = new PostMedia
                {
                    Id = Guid.NewGuid(),
                    PostId = post.Id,
                    Path = mediaPath
                };
                _context.PostMedia.Add(postMedia);
                
            }
        }

        _context.SaveChanges();
    }

    public void Edit(Guid id, CreatePostRequest model)
    {
        var post = _context.Post.Find(id);
        _mapper.Map(model, post);
        _context.Post.Update(post);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var post = _context.Post.Find(id);
        _context.Post.Remove(post);

        _context.PostMedia.RemoveRange(_context.PostMedia.Where(m => m.PostId == id).ToList());
        _context.SaveChanges();
    }

    #endregion Methods
}
