using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions
{
	public interface ICouponRepository : IGenericRepository<Coupon>
	{
		Task<bool> AddCouponAsync(string userId, Coupon coupon);
	}
}
