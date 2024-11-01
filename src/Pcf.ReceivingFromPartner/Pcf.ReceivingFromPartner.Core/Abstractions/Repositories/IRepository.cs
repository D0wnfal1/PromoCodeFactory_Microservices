using MongoDB.Bson;
using System.Linq.Expressions;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> GetByIdAsync(ObjectId id);
        
        Task<IEnumerable<T>> GetRangeByIdsAsync(List<ObjectId> ids);
        
        Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate);
        
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
        public void DeleteAll();
        public void AddMany(IEnumerable<T> entities);
	}
}