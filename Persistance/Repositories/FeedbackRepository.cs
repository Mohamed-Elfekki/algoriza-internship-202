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
	public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
	{
		private readonly ApplicationDbContext _context;

		public FeedbackRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
