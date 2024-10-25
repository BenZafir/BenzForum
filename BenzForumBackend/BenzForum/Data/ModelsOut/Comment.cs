using BenzForum.Data.BaseModels;

namespace BenzForum.Data.ModelsOut
{
    public class CommentResponse : BaseComment
    {
        public int Id { get; set; }
        public UserResponse User { get; set; }
    }
}
