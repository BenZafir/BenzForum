using System;
using System.ComponentModel.DataAnnotations;

namespace BenzForum.Data.BaseModels
{
    /// <summary>
    /// Represents the base model for a comment.
    /// </summary>
    public class BaseComment
    {
        private const int MAX_CHARACTERS = 1000;
        /// <summary>
        /// Gets or sets the ID of the post to which the comment belongs.
        /// </summary>
        [Required]
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who made the comment.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        [Required]
        [MaxLength(MAX_CHARACTERS, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content { get; set; }
    }
}
