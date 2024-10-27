using Microsoft.EntityFrameworkCore;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain;

namespace Pcf.Administration.DataAccess.Repositories
{
	public class EfRepository<T> : IRepository<T> where T: BaseEntity
	{
		private readonly DataContext _dataContext;
        public EfRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddAsync(T entity)
		{
			await _dataContext.Set<T>().AddAsync(entity);
			await _dataContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dataContext.Set<T>().Remove(entity);
			await _dataContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			var entities = await _dataContext.Set<T>().ToListAsync();
			return entities;
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			var entity = await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
			return entity;
		}

		public async Task<T> GetFirstWhere(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await _dataContext.Set<T>().FirstOrDefaultAsync(predicate);
		}

		public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
		{
			var entities = await _dataContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
			return entities;
		}

		public async Task<IEnumerable<T>> GetWhere(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await _dataContext.Set<T>().Where(predicate).ToListAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_dataContext.Set<T>().Update(entity);
			await _dataContext.SaveChangesAsync();
		}
	}
}
