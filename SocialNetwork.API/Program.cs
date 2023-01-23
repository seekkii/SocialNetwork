using Microsoft.EntityFrameworkCore;
using SocialNetwork.API.Authorization;
using SocialNetwork.API.Helpers;
using SocialNetwork.API.Hubs;
using SocialNetwork.API.Services;

string MyAllowSpecificOrigins = "Policy"; // !TODO

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    // use sql server db in production and sqlite db in development
    services.AddDbContext<DataContext>();

    services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          builder =>
                          {
                              builder.WithOrigins("http://localhost:8080")
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .AllowCredentials();
                          });
    });
    services.AddControllers();
    services.AddControllers().AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
    services.AddSignalR();

    // configure automapper with all automapper profiles from this assembly
    services.AddAutoMapper(typeof(Program));

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IPostService, PostService>();
    services.AddScoped<ICommentService, CommentService>();
    services.AddScoped<IChatService, ChatService>();
}

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(MyAllowSpecificOrigins);

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<CommentHub>("/commentsocket");
        endpoints.MapHub<PostHub>("/postsocket");
        endpoints.MapHub<ChatHub>("/chatsocket");
        endpoints.MapHub<NotificationHub>("/notificationsocket");
    });

    app.MapControllers();
}

app.Run("https://localhost:6868");