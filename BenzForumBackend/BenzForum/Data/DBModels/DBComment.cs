using BenzForum.Data.BaseModels;
using BenzForum.Data.EntityInterfaceModel;

namespace BenzForum.Models
{
    public class DBComment: BaseComment, IEntity
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }
        public DBPost Post { get; set; }
        public DBUser User { get; set; }
    }
}
