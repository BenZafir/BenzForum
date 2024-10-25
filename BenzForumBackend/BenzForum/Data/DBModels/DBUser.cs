using BenzForum.Data.BaseModels;
using BenzForum.Data.EntityInterfaceModel;

namespace BenzForum.Models
{
    public class DBUser : BaseUser, IEntity
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }
        public string PasswordHash { get; set; }
    }

}
