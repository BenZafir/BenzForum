using BenzForum.Data.BaseModels;
using BenzForum.Models;

namespace BenzForum.Data.ModelsOut
{
    public class PostResponse : BasePost
    {
        public int Id { get; set; }
        public UserResponse User { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }

    }

}
