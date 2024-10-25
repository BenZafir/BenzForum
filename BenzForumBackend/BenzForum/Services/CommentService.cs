using Microsoft.AspNetCore.SignalR;
using ForumApp.Hubs;
using ForumApp.Repositories;
using BenzForum.Models;
using BenzForum.Data.ModelsOut;
using BenzForum.Data.ModelsIn;
using BenzForum.Data.Factory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BenzForum.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<DBComment> _commentRepository;
        private readonly IRepository<DBUser> _userRepository;
        private readonly IModelConvertor _modelConvertor;
        private readonly ICommentHub _hubContext;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IRepository<DBComment> commentRepository, IRepository<DBUser> userRepository,
            ICommentHub hubContext, IModelConvertor modelConvertor, ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _modelConvertor = modelConvertor ?? throw new ArgumentNullException(nameof(modelConvertor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CommentResponse> AddCommentAsync(CommentRequest comment)
        {
            try
            {
                var dbComment = _modelConvertor.Convert<CommentRequest, DBComment>(comment);
                await _commentRepository.Add(dbComment);
                var user = await _userRepository.Get(u => u.Id == comment.UserId);
                var commentResponse = _modelConvertor.Convert<DBComment, CommentResponse>(dbComment);
                await _hubContext.SendComment(commentResponse);
                return commentResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a comment.");
                throw;
            }
        }

        public async Task<IEnumerable<CommentResponse>> GetAllCommentsAsync()
        {
            try
            {
                var dbComments = await _commentRepository.GetAll();
                return dbComments.Select(comment => _modelConvertor.Convert<DBComment, CommentResponse>(comment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all comments.");
                throw;
            }
        }

        public async Task<CommentResponse> GetCommentByIdAsync(int id)
        {
            try
            {
                var dbComment = await _commentRepository.Get(c => c.Id == id);
                return _modelConvertor.Convert<DBComment, CommentResponse>(dbComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the comment with id {id}.");
                throw;
            }
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsByPostIdAsync(int postId)
        {
            try
            {
                var dbComments = await _commentRepository.GetSome(c => c.PostId == postId);
                return dbComments.Select(comment => _modelConvertor.Convert<DBComment, CommentResponse>(comment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting comments for post id {postId}.");
                throw;
            }
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsByUserIdAsync(int userId)
        {
            try
            {
                var dbComments = await _commentRepository.GetSome(c => c.UserId == userId);
                return dbComments.Select(comment => _modelConvertor.Convert<DBComment, CommentResponse>(comment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting comments for user id {userId}.");
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(int id, int userId)
        {
            try
            {
                var comment = await GetCommentByIdAsync(id);
                if (comment.User.Id == userId)
                {
                    await _commentRepository.Delete(id);
                    await _hubContext.SendPostListUpdate("");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"User {userId} attempted to delete comment {id} without permission.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the comment with id {id}.");
                throw;
            }
        }
    }
}



