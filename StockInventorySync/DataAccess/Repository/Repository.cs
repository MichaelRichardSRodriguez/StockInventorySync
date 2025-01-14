using Microsoft.EntityFrameworkCore;
using StockInventorySync.DataAccess.Data;
using StockInventorySync.DataAccess.RepositoryInterfaces;
using System.Linq.Expressions;

namespace StockInventorySync.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{

		private readonly ApplicationDbContext _context;
		private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            
			_context = context;	
			_dbSet = _context.Set<T>();
			
        }
        public async Task CreateAsync(T entity)
		{
			 await _dbSet.AddAsync(entity);
		}

		public async Task DeleteAsync(T entity)
		{
			 _dbSet.Remove(entity);
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null,string? includeProperties = null)
		{

			var queryObject = _dbSet.AsQueryable();

			if (expression != null)
			{
				queryObject = queryObject.Where(expression);
			}

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
				{
					queryObject.Include(includeProp);
				}
			}

			return await queryObject.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
		}
	}
}
