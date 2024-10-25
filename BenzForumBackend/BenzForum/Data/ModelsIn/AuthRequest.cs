using BenzForum.Data.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BenzForum.Data.ModelsIn
{
    /// <summary>
    /// Represents an authentication request with username and password.
    /// </summary>
    public class AuthRequest : BaseUser
    {
        private const int MAX_PASSWORD_CHARACTERS = 30;

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required]
        [MaxLength(MAX_PASSWORD_CHARACTERS, ErrorMessage = "Password cannot exceed 30 characters.")]
        public string Password { get; set; }
    }
}

