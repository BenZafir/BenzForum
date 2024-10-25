using BenzForum.Data.EntityInterfaceModel;
using System.Linq.Expressions;

namespace ForumApp.Repositories
{
    public interface IRepository<DB> where DB : class, IEntity
    {
        Task<DB> Add(DB entity);
        Task Delete(int id);
        Task<DB> Get(Expression<Func<DB, bool>> predicate);
        Task<IEnumerable<DB>> GetAll();
        Task<IEnumerable<DB>> GetSome(Expression<Func<DB, bool>> predicate);
        Task<DB> Update(DB entity);
    }
}