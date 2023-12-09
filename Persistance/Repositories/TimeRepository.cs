using Application.Repositories.Abstractions;
using Domain.Entities;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
	public class TimeRepository : GenericRepository<Time>, ITimeRepository
	{
		public TimeRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
