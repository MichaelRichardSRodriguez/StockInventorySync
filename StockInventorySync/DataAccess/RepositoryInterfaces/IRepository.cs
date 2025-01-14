﻿using System.Linq.Expressions;

namespace StockInventorySync.DataAccess.RepositoryInterfaces
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? expression = null, string? includeProperties = null);
		Task<T> GetByIdAsync(int id);
		Task CreateAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		
	}
}