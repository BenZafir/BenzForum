using BenzForum.Data.Factory;
using BenzForum.Data.ModelsIn;
using BenzForum.Data.ModelsOut;
using BenzForum.Models;
using ForumApp.Hubs;
using ForumApp.Repositories;

namespace BenzForum.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<DBPost> _postRepository;
        private readonly IModelConvertor _modelConvertor;
        private readonly ICommentHub _hubContext;
        private readonly IRepository<DBUser> _userRepository;
        private readonly ILogger<PostService> _logger;

        public PostService(IRepository<DBPost> postRepository, IModelConvertor modelConvertor,
            IRepository<DBUser> userRepository, ICommentHub hubContext, ILogger<PostService> logger)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _modelConvertor = modelConvertor ?? throw new ArgumentNullException(nameof(modelConvertor));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PostResponse> AddPostAsync(PostRequest post)
        {
            try
            {
                var dbPost = _modelConvertor.Convert<PostRequest, DBPost>(post);
                await _postRepository.Add(dbPost);
                var user = await _userRepository.Get(u => u.Id == post.UserId);
                var postResponse = _modelConvertor.Convert<DBPost, PostResponse>(dbPost);
                await _hubContext.SendPost(postResponse);
                return postResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a post.");
                throw;
            }
        }

        public async Task<IEnumerable<PostResponse>> GetAllPostsAsync()
        {
            try
            {
                var dbPosts = await _postRepository.GetAll();
                return dbPosts.Select(post => _modelConvertor.Convert<DBPost, PostResponse>(post));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all posts.");
                throw;
            }
        }

        public async Task<PostResponse> GetPostByIdAsync(int id)
        {
            try
            {
                var dbPost = await _postRepository.Get(p => p.Id == id);
                return _modelConvertor.Convert<DBPost, PostResponse>(dbPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the post with id {id}.");
                throw;
            }
        }

        public async Task<IEnumerable<PostResponse>> GetPostsByUserIdAsync(int userId)
        {
            try
            {
                var dbPosts = await _postRepository.GetSome(p => p.UserId == userId);
                return dbPosts.Select(post => _modelConvertor.Convert<DBPost, PostResponse>(post));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting posts for user id {userId}.");
                throw;
            }
        }

        public async Task<PostResponse> UpdatePostAsync(PostRequest post)
        {
            try
            {
                var dbPost = _modelConvertor.Convert<PostRequest, DBPost>(post);
                await _postRepository.Update(dbPost);
                return _modelConvertor.Convert<DBPost, PostResponse>(dbPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a post.");
                throw;
            }
        }

        public async Task<bool> DeletePostAsync(int postId, int userId)
        {
            try
            {
                var post = await GetPostByIdAsync(postId);
                if (post.User.Id == userId)
                {
                    await _postRepository.Delete(postId);
                    await _hubContext.SendPostListUpdate("");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"User {userId} attempted to delete post {postId} without permission.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the post with id {postId}.");
                throw;
            }
        }
    }
}
