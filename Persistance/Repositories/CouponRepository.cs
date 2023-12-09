using Application.Repositories.Abstractions;
using Domain.Entities;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
	public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public CouponRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<bool> AddCouponAsync(string userId, Coupon coupon)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).FirstOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					coupon.AdminId = isUserAnAdmin.AdminId;
					var result = await _context.Coupons.AddAsync(coupon);
					if (result != null)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
