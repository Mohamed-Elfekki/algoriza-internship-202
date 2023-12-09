using Application.DTOs.Respones;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
	public interface IBookingService
	{
		Task<IDictionary<string, object>> GetNumOfBookingsAsync(string userId);
		Task<string> CreateBookingAsync(string userId, int AppointmentId, int timeId, string? couponCode);
		Task<string> CancelBookingAsync(string userId, int bookingId);

		Task<IEnumerable<BookingDtoRes>> GetAllBookingByIdAsync(string userId);
		Task<IEnumerable<BookingByDateDtoRes>> GetAllBookingByDateAsync(string userId, int requiredpage, int pageSize, DateTime dateTime);
		Task<string> ConfirmCheckUpAsync(string userId, int bookingId);
	}
}
