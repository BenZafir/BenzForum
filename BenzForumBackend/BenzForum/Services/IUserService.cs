using BenzForum.Data.ModelsIn;

namespace ForumApp.Services
{
    public interface IUserService
    {
        Task<string> Login(AuthRequest loginUser);
        Task Register(AuthRequest registerUser);
    }
}