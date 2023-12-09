using Application.Repositories.Abstractions;
using Domain.Entities.Credentials;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		private DbSet<T> table;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
			table = _context.Set<T>();
		}




		public async Task<IEnumerable<T>> GetAllAsync()
		{

			var x = await table.ToListAsync();

			if (x.Count == 0)
			{
				return null;
			}
			return x;
		}

		public async Task<T> GetByIdAsync(int id)
		{
			var x = await table.FindAsync(id);

			if (x == null)
			{
				return null;
			}
			return x;
		}

		public async Task<IEnumerable<T>> GetAllByIdAsync(Expression<Func<T, bool>> filter)
		{
			var x = await table.Where(filter).ToListAsync();
			if (x == null)
			{
				return null;
			}
			return x;
		}

		public async Task<bool> CreateAsync(T entity)
		{
			var result = await table.AddAsync(entity);
			if (result != null)
			{
				return true;
			}
			return false;
		}

		public bool Update(T entity)
		{
			var x = table.Update(entity);
			if (x != null)
			{
				return true;
			}
			return false;
		}

		public bool Delete(T entity)
		{
			var result = table.Remove(entity);
			if (result != null)
			{
				return true;
			}
			return false;
		}

	}
}
