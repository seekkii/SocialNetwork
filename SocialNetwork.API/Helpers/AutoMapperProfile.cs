using AutoMapper;
using SocialNetwork.API.Entities.Chat;
using SocialNetwork.API.Entities.Comment;
using SocialNetwork.API.Entities.Post;
using SocialNetwork.API.Entities.User;
using SocialNetwork.API.Models.Chat;
using SocialNetwork.API.Models.Post;
using SocialNetwork.API.Models.User;

namespace SocialNetwork.API.Helpers;

/// <summary>
/// Handle models to be update into DB
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region User
        // User -> AuthenticateResponse
        CreateMap<User, AuthenticateResponse>();

        // RegisterRequest -> User
        CreateMap<RegisterRequest, User>();

        // UpdateCredentialsRequest -> User
        CreateMap<UpdateCredentialsRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));

        // UpdateProfileRequest -> UserProfile
        CreateMap<UpdateProfileRequest, UserProfile>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));

        // UpdateSettingRequest -> UserSetting
        CreateMap<UpdateSettingRequest, UserSetting>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));
        #endregion User

        #region Post
        // CreatePostRequest -> Post
        CreateMap<CreatePostRequest, Post>()
            .ForSourceMember(x => x.MediaPaths, y => y.DoNotValidate());
        
        // CreateCommentRequest -> Comment
        CreateMap<CreateCommentRequest, Comment>()
            .ForSourceMember(x => x.MediaPaths, y => y.DoNotValidate());

        #endregion Post
        
        #region Chat
        // CreateMessageRequest -> Message
        CreateMap<CreateMessageRequest, Message>()
            .ForSourceMember(x => x.MediaPaths, y => y.DoNotValidate());
        #endregion Chat
    }
}
