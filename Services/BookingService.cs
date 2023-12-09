using Application.DTOs.Respones;
using Application.Repositories.Abstractions.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Credentials;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	internal sealed class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public BookingService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mapper = mapper;
		}



		public async Task<IDictionary<string, object>> GetNumOfBookingsAsync(string userId)
		{
			var result = await _unitOfWork.Bookings.GetNumOfBookingsAsync(userId);
			return result;
		}

		public async Task<string> CreateBookingAsync(string userId, int AppointmentId, int timeId, string? couponCode)
		{
			var booking = await _unitOfWork.Bookings.CreateBookingAsync(userId, AppointmentId, timeId, couponCode);
			if (booking == "Ok")
			{
				_unitOfWork.commit();
				return booking;
			}
			return booking;
		}

		public async Task<string> CancelBookingAsync(string userId, int bookingId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isBookingExist = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
				if (isBookingExist != null)
				{
					isBookingExist.Status = Status.Canceled.ToString();
					var CanceldBooking = _unitOfWork.Bookings.Update(isBookingExist);
					if (CanceldBooking == true)
					{
						_unitOfWork.commit();
						return "Ok";
					}
					return "Faild To Cancel Booking";
				}
				return "Booking Not Found";
			}
			return "User Not Found";
		}

		public async Task<IEnumerable<BookingDtoRes>> GetAllBookingByIdAsync(string userId)
		{
			var bookings = await _unitOfWork.Bookings.GetAllBookingByIdAsync(userId);
			if (bookings != null)
			{
				var result = _mapper.Map<ICollection<BookingDtoRes>>(bookings);
				if (result != null)
				{
					return result;
				}
				return null;
			}
			return null;
		}

		public async Task<IEnumerable<BookingByDateDtoRes>> GetAllBookingByDateAsync(string userId, int requiredpage, int pageSize, DateTime dateTime)
		{
			var bookings = await _unitOfWork.Bookings.GetAllBookingByDateAsync(userId, requiredpage, pageSize, dateTime);
			if (bookings != null)
			{
				var result = _mapper.Map<ICollection<BookingByDateDtoRes>>(bookings);
				if (result != null)
				{
					return result;
				}
				return null;
			}
			return null;
		}

		public async Task<string> ConfirmCheckUpAsync(string userId, int bookingId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserADoctor = await _unitOfWork.Doctors.GetDoctorByIdAsync(userId);
				if (isUserADoctor != null)
				{
					var isBookingExist = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
					if (isBookingExist != null)
					{
						isBookingExist.Status = Status.Completed.ToString();
						var CompletedBooking = _unitOfWork.Bookings.Update(isBookingExist);

						if (CompletedBooking == true)
						{
							if (isBookingExist.CouponId != null)
							{
								var coupon = await _unitOfWork.Coupons.GetByIdAsync((int)isBookingExist.CouponId);
								if (coupon != null)
								{
									coupon.CompletedRequests += 1;
									_unitOfWork.Coupons.Update(coupon);
								}
							}
							_unitOfWork.commit();
							return "OK";
						}
						return "Faild To Confirm Booking";
					}
					return "Booking Not Found";
				}
			}
			return "User Not Found";
		}


	}
}
