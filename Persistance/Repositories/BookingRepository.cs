using Application.DTOs.Respones;
using Application.DTOs.Respones.Users;
using Application.Repositories.Abstractions;
using Domain.Entities;
using Domain.Entities.Credentials;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
	public class BookingRepository : GenericRepository<Booking>, IBookingRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public BookingRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}


		public async Task<IDictionary<string, object>> GetNumOfBookingsAsync(string userId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAnAdmin = await _context.Admins.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAnAdmin != null)
				{
					var bookings = await _context.Bookings.CountAsync();
					var pendingBookings = await _context.Bookings.Where(x => x.Status == "Pending").CountAsync();
					var completedBookings = await _context.Bookings.Where(x => x.Status == "Completed").CountAsync();
					var canceledBookings = await _context.Bookings.Where(x => x.Status == "Canceled").CountAsync();
					IDictionary<string, object> pairs = new Dictionary<string, object>();
					pairs.Add("Total Requests", bookings);
					pairs.Add("Pending Requests", pendingBookings);
					pairs.Add("Completed Requests", completedBookings);
					pairs.Add("Canceled Requests", canceledBookings);
					return pairs;
				}
			}
			return null;
		}

		public async Task<string> CreateBookingAsync(string userId, int appointmentId, int timeId, string? couponCode)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserAPatient = await _context.Patients.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserAPatient != null)
				{
					var isAppointmentExist = await _context.Appointments.Where(x => x.AppointmentId == appointmentId).SingleOrDefaultAsync();
					if (isAppointmentExist != null)
					{
						var isTimeExist = await _context.Times.Where(x => x.TimeId == timeId).SingleOrDefaultAsync();
						if (isTimeExist != null)
						{
							var isCouponExist = await _context.Coupons.Where(x => x.DisountCode == couponCode).SingleOrDefaultAsync();
							Booking booking = new Booking()
							{
								AppointmentId = appointmentId,
								PatientId = isUserAPatient.PatientId,
								TimeId = timeId
							};
							if (isCouponExist != null)
							{
								if (isCouponExist.Deactivate == false)
								{
									booking.CouponId = isCouponExist.CouponId;
								}

							}
							var result = await _context.Bookings.AddAsync(booking);

							if (result != null)
							{
								return "Ok";
							}
							return "Failed To Create Booking";
						}
						return "Time Not Found";
					}
					return "Appointment Not Found";
				}
				return "User Not A Patient";
			}
			return "User Not Exist";
		}

		public async Task<IEnumerable<Booking>> GetAllBookingByIdAsync(string userId)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var bookings = await _context.Bookings.Include(x => x.Coupon).Include(x => x.Appointment).Include(x => x.Time).Where(x => x.Patient.User.Id == userId).ToListAsync();
				if (bookings.Count() > 0)
				{
					foreach (var item in bookings)
					{
						item.Appointment.Doctor = _context.Doctors.Where(x => x.DoctorId == item.Appointment.DoctorId).SingleOrDefault();
						item.Appointment.Doctor.Specialization = _context.Doctors.Where(x => x.SpecializationId == item.Appointment.Doctor.SpecializationId).Select(x => x.Specialization).SingleOrDefault();

					}
					return bookings;
				}
				return null;
			}
			return null;
		}

		public async Task<IEnumerable<Booking>> GetAllBookingByDateAsync(string userId, int requiredPage, int pageSize, DateTime dateTime)
		{
			var isUserExist = await _userManager.FindByIdAsync(userId);
			if (isUserExist != null)
			{
				var isUserADoctor = await _context.Doctors.Where(x => x.UserId == userId).SingleOrDefaultAsync();
				if (isUserADoctor != null)
				{
					decimal rowCount = await _context.Bookings.Where(x => x.Appointment.Doctor.User.Id == userId).CountAsync();
					if (pageSize <= 0)
					{
						pageSize = 1;
					}
					var pagesCount = Math.Ceiling(rowCount / pageSize);
					if (requiredPage > pagesCount)
					{
						requiredPage = 1;
					}
					if (requiredPage <= 0)
					{
						requiredPage = 1;
					}

					int skipCount = (requiredPage - 1) * pageSize;
					var bookings = await _context.Bookings.Include(x => x.Appointment).Include(x => x.Patient).Include(x => x.Time).Where(x => x.Appointment.Doctor.User.Id == userId && x.Time.DateTime == dateTime).Skip(skipCount).Take(pageSize).ToListAsync();
					foreach (var item in bookings)
					{
						var user = await _userManager.FindByIdAsync(item.Patient.UserId);
						item.Patient.User = user;
					}
					if (bookings.Count > 0)
					{
						return bookings;
					}
					return null;
				}
				return null;
			}
			return null;
		}

	}
}
