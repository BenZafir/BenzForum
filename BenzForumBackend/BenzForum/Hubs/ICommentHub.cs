
using BenzForum.Data.ModelsOut;

namespace ForumApp.Hubs
{
    public interface ICommentHub
    {
        Task SendComment(CommentResponse comment);
        Task SendPost(PostResponse post);
        Task SendPostListUpdate(string note);
        
    }
}