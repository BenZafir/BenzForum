using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BenzForum.Services;
using ForumApp.Services;
using BenzForum.Data.ModelsIn;
using BenzForum.Data.ModelsOut;
using BenzForum.Data.Factory;

namespace ForumApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IModelConvertor _modelConvertor;
        private readonly IUserService _userService;
        private readonly ILogger<PostsController> _logger;


        public PostsController(IPostService postService, IModelConvertor modelConvertor, IUserService userService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _modelConvertor = modelConvertor;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="createPostRequest">The post details.</param>
        /// <returns>Action result indicating the outcome of the post creation.</returns>
        [HttpPost]
        public async Task<ActionResult<PostResponse>> CreatePost(PostRequest createPostRequest)
        {
            try
            {
                // Extract UserId from JWT token
                var userIdClaim = User.FindFirst("Id");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in token." });
                }

                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new { Message = "Invalid user ID." });
                }

                createPostRequest.UserId = userId;
                var result = await _postService.AddPostAsync(createPostRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while creating a post");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>Action result with a list of all posts.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetAllPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving all posts");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <returns>Action result with the post details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponse>> GetPostById(int id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound(new { Message = "Post not found." });
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving the post by ID");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves posts by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Action result with a list of posts for the specified user.</returns>
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetPostsByUserId(int userId)
        {
            try
            {
                var posts = await _postService.GetPostsByUserIdAsync(userId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while retrieving posts by user ID");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <returns>Action result indicating the outcome of the post deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
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

                var result = await _postService.DeletePostAsync(id, userId);
                if (result)
                {
                    return Ok(new { Message = "Post deleted successfully." });
                }
                return Unauthorized(new { Message = "Not your post, you can't delete it." });
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while deleting the post");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}

