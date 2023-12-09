using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> GetAllByIdAsync(Expression<Func<T, bool>> filter);

		Task<T> GetByIdAsync(int id);

		Task<bool> CreateAsync(T entity);

		bool Update(T entity);
		bool Delete(T entity);

	}
}
