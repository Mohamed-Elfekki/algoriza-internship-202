using Domain.Entities;
using Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Abstractions
{
	public interface IBookingRepository : IGenericRepository<Booking>
	{
		Task<IDictionary<string, object>> GetNumOfBookingsAsync(string userId);
		Task<string> CreateBookingAsync(string userId, int appointmentId, int timeId, string? couponCode);
		Task<IEnumerable<Booking>> GetAllBookingByIdAsync(string userId);

		Task<IEnumerable<Booking>> GetAllBookingByDateAsync(string userId, int requiredPage, int pageSize, DateTime dateTime);
	}
}
