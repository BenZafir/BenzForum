using BenzForum.Data.ModelsIn;
using BenzForum.Models;

namespace BenzForum.Helpers
{
    public interface IJwtUtils
    {
        string GenerateToken(DBUser user);
    }
}
