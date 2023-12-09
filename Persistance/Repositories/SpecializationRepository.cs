using Application.Repositories.Abstractions;
using Domain.Entities;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
	public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public SpecializationRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IEnumerable<KeyValuePair<string, int>>> GetTo5SpecailizationsAsync(string userId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					IDictionary<string, int> result = new Dictionary<string, int>();
					var numOfSpecialize = await _context.Doctors.Select(x => x.SpecializationId).Distinct().ToListAsync();
					foreach (var item in numOfSpecialize)
					{
						var name = await _context.Specializations.Where(x => x.SpecializationId == item)
							.Select(x => x.SpecializationName).SingleOrDefaultAsync();
						var amount = await _context.Doctors.Where(x => x.SpecializationId == item).CountAsync();

						result.Add(name, amount);
					}
					var top = result.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value).Take(5);
					return top;
				}
			}
			return null;
		}
	}
}
