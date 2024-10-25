using BenzForum.Data.BaseModels;
using BenzForum.Data.EntityInterfaceModel;

namespace BenzForum.Models
{
    public class DBPost : BasePost, IEntity
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }
        public DBUser User { get; set; }
        public ICollection<DBComment> Comments { get; set; }
    }

}
