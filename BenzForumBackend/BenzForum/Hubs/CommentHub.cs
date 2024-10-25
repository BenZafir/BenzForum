using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using BenzForum.Data.ModelsOut;

namespace ForumApp.Hubs
{
    public class CommentHub : Hub, ICommentHub
    {
        private readonly IHubContext<CommentHub> _context;
        private readonly ILogger<CommentHub> _logger;

        public CommentHub(IHubContext<CommentHub> context, ILogger<CommentHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SendComment(CommentResponse comment)
        {
            try
            {
                await _context.Clients.All.SendAsync("ReceiveComment", comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending comment");
                throw;
            }
        }

        public async Task SendPost(PostResponse post)
        {
            try
            {
                await _context.Clients.All.SendAsync("ReceivePost", post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending post");
                throw;
            }
        }

        public async Task SendPostListUpdate(string note)
        {
            try
            {
                await _context.Clients.All.SendAsync("PostListUpdate", note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending post list update");
                throw;
            }
        }
    }
}