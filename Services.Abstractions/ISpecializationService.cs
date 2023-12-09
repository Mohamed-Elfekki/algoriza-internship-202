using Application.Repositories.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
	public interface ISpecializationService
	{
		Task<IEnumerable<KeyValuePair<string, int>>> GetTo5SpecailizationsAsync(string userId);
	}
}
