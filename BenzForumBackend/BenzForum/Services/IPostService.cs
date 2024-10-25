using BenzForum.Data.ModelsIn;
using BenzForum.Data.ModelsOut;

namespace BenzForum.Services
{
    public interface IPostService
    {
        Task<PostResponse> AddPostAsync(PostRequest post);
        Task<bool> DeletePostAsync(int postId,int userId);
        Task<IEnumerable<PostResponse>> GetAllPostsAsync();
        Task<PostResponse> GetPostByIdAsync(int id);
        Task<IEnumerable<PostResponse>> GetPostsByUserIdAsync(int userId);
        Task<PostResponse> UpdatePostAsync(PostRequest post);
    }
}