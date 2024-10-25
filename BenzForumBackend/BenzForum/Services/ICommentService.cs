using BenzForum.Data.ModelsIn;
using BenzForum.Data.ModelsOut;

namespace BenzForum.Services
{
    public interface ICommentService
    {
        Task<CommentResponse> AddCommentAsync(CommentRequest comment);
        Task<bool> DeleteCommentAsync(int id,int userId);
        Task<IEnumerable<CommentResponse>> GetAllCommentsAsync();
        Task<CommentResponse> GetCommentByIdAsync(int id);
        Task<IEnumerable<CommentResponse>> GetCommentsByPostIdAsync(int postId);
        Task<IEnumerable<CommentResponse>> GetCommentsByUserIdAsync(int userId);
    }
}