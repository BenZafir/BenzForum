using System;
using System.ComponentModel.DataAnnotations;

namespace BenzForum.Data.BaseModels
{
    /// <summary>
    /// Represents the base model for a post.
    /// </summary>
    public class BasePost
    {
        private const int MAX_TITLE_CHARACTERS = 200;
        private const int MAX_CHARACTERS = 3000;

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        [Required]
        [MaxLength(MAX_TITLE_CHARACTERS, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        [Required]
        [MaxLength(MAX_CHARACTERS, ErrorMessage = "Content cannot exceed 3000 characters.")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the post.
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }
}

