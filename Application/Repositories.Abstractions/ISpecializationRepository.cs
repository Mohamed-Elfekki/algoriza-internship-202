using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions
{
	public interface ISpecializationRepository : IGenericRepository<Specialization>
	{
		Task<IEnumerable<KeyValuePair<string, int>>> GetTo5SpecailizationsAsync(string userId);
	}
}
