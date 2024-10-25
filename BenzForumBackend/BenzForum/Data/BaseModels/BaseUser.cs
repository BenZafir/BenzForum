using System.ComponentModel.DataAnnotations;

namespace BenzForum.Data.BaseModels
{
    public class BaseUser
    {
        private const int MAX_USERNAME_CHARACTERS = 50;

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        [MaxLength(MAX_USERNAME_CHARACTERS, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; }
    }

}
