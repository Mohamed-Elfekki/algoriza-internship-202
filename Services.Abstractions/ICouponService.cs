using Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
	public interface ICouponService
	{
		Task<bool> AddCouponAsync(string userId, CouponDtoReq dto);
		Task<bool> UpdateCouponAsync(int couponId, CouponDtoReq dto);
		Task<bool> DeleteCouponAsync(int couponId);
		Task<bool> DeactivateCouponAsync(int couponId);
	}
}
