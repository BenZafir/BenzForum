using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BenzForum.Services;
using BenzForum.Data.ModelsIn;
using BenzForum.Data.ModelsOut;
using Microsoft.Extensions.Logging;

namespace ForumApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentsController> _logger;


        public CommentsController(ICommentService commentService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new comment.
        /// </summary>
        /// <param name="commentRequest">The comment details.</param>
        /// <returns>Action result indicating the outcome of the comment addition.</returns>
        [HttpPost]
        public async Task<ActionResult<CommentResponse>> AddComment(CommentRequest commentRequest)
        {
            try
            {
                var userIdClaim = User.FindFirst("Id");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in token." });
                }

                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new { Message = "Invalid user ID." });
                }

                commentRequest.UserId = userId;
                var result = await _commentService.AddCommentAsync(commentRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while adding a comment");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves all comments.
        /// </summary>
        /// <returns>Action result with a list of all comments.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAllComments()
        {
            try
            {
                var comments = await _commentService.GetAllCommentsAsync();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving all comments");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The comment ID.</param>
        /// <returns>Action result with the comment details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponse>> GetCommentById(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return NotFound(new { Message = "Comment not found." });
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving the comment by ID");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves comments by post ID.
        /// </summary>
        /// <param name="postId">The post ID.</param>
        /// <returns>Action result with a list of comments for the specified post.</returns>
        [HttpGet("Post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByPostId(int postId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByPostIdAsync(postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving comments by post ID");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves comments by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Action result with a list of comments for the specified user.</returns>
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByUserId(int userId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByUserIdAsync(userId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving comments by user ID");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The comment ID.</param>
        /// <returns>Action result indicating the outcome of the comment deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst("Id");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in token." });
                }

                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new { Message = "Invalid user ID." });
                }

                var result = await _commentService.DeleteCommentAsync(id, userId);
                if (result)
                {
                    return Ok(new { Message = "Comment deleted successfully." });
                }
                return Unauthorized(new { Message = "Not your comment, you can't delete it." });
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while deleting the comment");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}
