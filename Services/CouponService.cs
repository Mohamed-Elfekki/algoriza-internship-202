using Application.DTOs.Requests;
using Application.Repositories.Abstractions.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Credentials;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Services
{
	internal sealed class CouponService : ICouponService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CouponService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> AddCouponAsync(string userId, CouponDtoReq dto)
		{
			Coupon coupon = new Coupon() { Discount = dto.Discount, DiscountValue = dto.DiscountValue, CompletedRequests = 0 };
			var result = await _unitOfWork.Coupons.AddCouponAsync(userId, coupon);
			if (result is true)
			{
				_unitOfWork.commit();
				return true;
			}
			return false;
		}

		public async Task<bool> UpdateCouponAsync(int couponId, CouponDtoReq dto)
		{
			var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
			if (coupon is not null)
			{
				if (coupon.CompletedRequests == 0)
				{
					coupon.DiscountValue = dto.DiscountValue;
					coupon.Discount = dto.Discount;
					var result = _unitOfWork.Coupons.Update(coupon);
					if (result is true)
					{
						_unitOfWork.commit();
						return true;
					}
				}
			}
			return false;
		}

		public async Task<bool> DeleteCouponAsync(int couponId)
		{
			var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
			if (coupon is not null)
			{
				if (coupon.CompletedRequests == 0)
				{
					var result = _unitOfWork.Coupons.Delete(coupon);
					if (result is true)
					{
						_unitOfWork.commit();
						return true;
					}
				}
			}
			return false;
		}


		public async Task<bool> DeactivateCouponAsync(int couponId)
		{
			var coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
			if (coupon is not null)
			{
				if (coupon.CompletedRequests == 0)
				{
					coupon.Deactivate = true;
					var result = _unitOfWork.Coupons.Update(coupon);
					if (result is true)
					{
						_unitOfWork.commit();
						return true;
					}
				}
			}
			return false;
		}
	}
}
