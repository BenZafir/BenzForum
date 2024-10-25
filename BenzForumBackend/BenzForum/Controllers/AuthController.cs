using Microsoft.AspNetCore.Mvc;
using ForumApp.Services;
using BenzForum.Data.ModelsIn;
using Microsoft.Extensions.Logging;

namespace ForumApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user details for registration.</param>
        /// <returns>Action result indicating the outcome of the registration.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRequest user)
        {
            try
            {
                await _userService.Register(user);
                return Ok(new { Message = "Registration successful" });
            }
            catch (InvalidOperationException ex)
            {
                // Log a warning when the user already exists
                _logger.LogWarning(ex, "User already exists: {Username}", user.Username);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred during registration");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="loginRequest">The login details.</param>
        /// <returns>Action result with the authentication token if successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest loginRequest)
        {
            try
            {
                var token = await _userService.Login(loginRequest);
                if (token == null)
                {
                    // Log a warning for unauthorized login attempts
                    _logger.LogWarning("Unauthorized login attempt for user: {Username}", loginRequest.Username);
                    return Unauthorized(new { Message = "Invalid username or password" });
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Log an error for unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred during login");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}
